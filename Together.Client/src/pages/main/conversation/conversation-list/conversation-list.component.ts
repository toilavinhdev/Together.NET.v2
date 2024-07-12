import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ConversationService, UserService } from '@/shared/services';
import {
  IConversationViewModel,
  IListConversationRequest,
} from '@/shared/entities/conversation.entities';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { AvatarComponent } from '@/shared/components/elements';
import { TimeAgoPipe } from '@/shared/pipes';
import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'together-conversation-list',
  standalone: true,
  imports: [AvatarComponent, TimeAgoPipe, NgIf, NgClass, AsyncPipe],
  templateUrl: './conversation-list.component.html',
})
export class ConversationListComponent extends BaseComponent implements OnInit {
  params: IListConversationRequest = {
    pageIndex: 1,
    pageSize: 12,
  };

  constructor(
    protected conversationService: ConversationService,
    protected userService: UserService,
  ) {
    super();
  }

  ngOnInit() {
    this.loadConversations();
  }

  private loadConversations() {
    this.conversationService
      .listConversation(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result }) => {
          this.conversationService.conversations$.next(result);
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }
}
