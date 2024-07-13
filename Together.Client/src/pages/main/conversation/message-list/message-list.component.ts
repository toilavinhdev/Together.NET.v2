import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ActivatedRoute } from '@angular/router';
import { filter, take, takeUntil } from 'rxjs';
import {
  ConversationService,
  MessageService,
  UserService,
  WebSocketService,
} from '@/shared/services';
import { IListMessageRequest } from '@/shared/entities/message.entities';
import { getErrorMessage } from '@/shared/utilities';
import { AvatarComponent } from '@/shared/components/elements';
import { TooltipModule } from 'primeng/tooltip';
import { TimeAgoPipe } from '@/shared/pipes';
import { Button } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { AsyncPipe, NgIf } from '@angular/common';
import { EConversationType } from '@/shared/enums';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { websocketClientTarget } from '@/shared/constants';

@Component({
  selector: 'together-message-list',
  standalone: true,
  imports: [
    AvatarComponent,
    TooltipModule,
    TimeAgoPipe,
    Button,
    InputTextModule,
    NgIf,
    ReactiveFormsModule,
    AsyncPipe,
  ],
  templateUrl: './message-list.component.html',
})
export class MessageListComponent
  extends BaseComponent
  implements OnInit, AfterViewChecked
{
  protected readonly EConversationType = EConversationType;

  extra: { [key: string]: any } = {};

  params: IListMessageRequest = {
    conversationId: '',
    pageIndex: 1,
    pageSize: 24,
  };

  messageForm!: UntypedFormGroup;

  constructor(
    private activatedRoute: ActivatedRoute,
    protected messageService: MessageService,
    private formBuilder: UntypedFormBuilder,
    protected userService: UserService,
    private conversationService: ConversationService,
    private webSocketService: WebSocketService,
  ) {
    super();
  }

  ngOnInit() {
    this.buildForm();
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        this.params.conversationId = paramMap.get('conversationId')!;
        this.loadMessages();
      });
    this.listenWebSocket();
  }

  ngAfterViewChecked() {
    if (this.params.pageIndex > 1) return;
    this.scrollToBottom();
  }

  private loadMessages() {
    this.messageService
      .listMessage(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, extra }) => {
          this.messageService.messages$.next(result.reverse());
          this.extra = extra;
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  private buildForm() {
    this.messageForm = this.formBuilder.group({
      text: [null, [Validators.required]],
    });
  }

  private scrollToBottom() {
    const wrapper = document.getElementById('messages-wrapper');
    if (!wrapper) return;
    wrapper.scrollTop = wrapper.scrollHeight;
  }

  onSendMessage() {
    if (this.messageForm.invalid) return;
    this.messageService
      .sendMessage({
        ...this.messageForm.value,
        conversationId: this.params.conversationId,
      })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.messageForm.reset();
          this.userService.me$.pipe(take(1)).subscribe((me) => {
            if (!me) return;
            // Update messages
            this.messageService.messages$
              .pipe(take(1))
              .subscribe((messages) => {
                this.messageService.messages$.next([
                  ...messages,
                  {
                    ...data,
                    createdById: me.id,
                    createdByUserName: me.userName,
                    createdByAvatar: me.avatar,
                  },
                ]);
              });
            // Update conversations
            this.conversationService.conversations$
              .pipe(take(1))
              .subscribe((conversations) => {
                const existed = conversations.find(
                  (c) => c.id == this.params.conversationId,
                );
                if (!existed) {
                } else {
                  this.conversationService.conversations$.next([
                    {
                      ...existed,
                      lastMessageAt: data.createdAt,
                      lastMessageByUserId: me.id,
                      lastMessageByUserName: me.userName,
                      lastMessageText: data.text,
                    },
                    ...conversations.filter(
                      (c) => c.id !== this.params.conversationId,
                    ),
                  ]);
                }
              });
          });
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  private listenWebSocket() {
    this.webSocketService.client$
      .pipe(
        takeUntil(this.destroy$),
        filter(
          (message) => message.target === websocketClientTarget.ReceivedMessage,
        ),
      )
      .subscribe({
        next: (socket) => {
          if (socket.message.conversationId !== this.params.conversationId)
            return;
          // add message
          this.messageService.messages$.pipe(take(1)).subscribe((messages) => {
            this.messageService.messages$.next([
              ...messages,
              {
                ...socket.message,
                createdById: socket.message.createdById,
                createdByUserName: socket.message.userName,
                createdByAvatar: socket.message.avatar,
              },
            ]);
          });
        },
      });
  }
}
