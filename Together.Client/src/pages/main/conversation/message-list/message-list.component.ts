import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ActivatedRoute } from '@angular/router';
import { take, takeUntil } from 'rxjs';
import {
  ConversationService,
  MessageService,
  UserService,
} from '@/shared/services';
import {
  IListMessageRequest,
  IMessageViewModel,
} from '@/shared/entities/message.entities';
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

  messages: IMessageViewModel[] = [];

  extra: { [key: string]: any } = {};

  params: IListMessageRequest = {
    conversationId: '',
    pageIndex: 1,
    pageSize: 24,
  };

  messageForm!: UntypedFormGroup;

  constructor(
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private formBuilder: UntypedFormBuilder,
    protected userService: UserService,
    private conversationService: ConversationService,
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
  }

  ngAfterViewChecked() {
    if (this.params.pageIndex > 1) return;
    this.scrollToBottom();
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
            this.messages = [
              ...this.messages,
              {
                ...data,
                createdById: me.id,
                createdByUserName: me.userName,
                createdByAvatar: me.avatar,
              },
            ];
            // Update conversations
            this.conversationService.conversations$
              .pipe(take(1))
              .subscribe((conversations) => {
                this.conversationService.conversations$.next(
                  conversations.map((conversation) => {
                    if (conversation.id === this.params.conversationId) {
                      return {
                        ...conversation,
                        image: me.avatar,
                        lastMessageAt: data.createdAt,
                        lastMessageByUserId: me.id,
                        lastMessageByUserName: me.userName,
                        lastMessageText: data.text,
                      };
                    }
                    return conversation;
                  }),
                );
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

  private loadMessages() {
    this.messageService
      .listMessage(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, extra }) => {
          this.messages = result.reverse();
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
}
