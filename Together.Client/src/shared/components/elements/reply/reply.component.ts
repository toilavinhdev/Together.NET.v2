import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { IReplyViewModel } from '@/shared/entities/reply.entities';
import { NgClass, NgForOf, NgIf } from '@angular/common';
import { BaseComponent } from '@/core/abstractions';
import {
  SanitizeHtmlPipe,
  ShortenNumberPipe,
  TimeAgoPipe,
} from '@/shared/pipes';
import {
  AvatarComponent,
  ReplyWriterComponent,
  VoteComponent,
} from '@/shared/components/elements';
import { ReplyService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-reply',
  standalone: true,
  imports: [
    NgClass,
    SanitizeHtmlPipe,
    AvatarComponent,
    TimeAgoPipe,
    ShortenNumberPipe,
    NgIf,
    VoteComponent,
    NgForOf,
    ReplyWriterComponent,
  ],
  templateUrl: './reply.component.html',
})
export class ReplyComponent extends BaseComponent {
  @Input()
  reply!: IReplyViewModel;

  children: IReplyViewModel[] = [];

  childrenNextPageIndex = 1;

  childrenHasNextPage = true;

  childrenPageSize = 1;

  childrenLoading = false;

  constructor(
    private replyService: ReplyService,
    private cdk: ChangeDetectorRef,
  ) {
    super();
  }

  showChildren() {
    if (this.childrenLoading) return;
    this.loadChildren();
  }

  onAddChild(reply: IReplyViewModel) {
    this.children = [reply, ...this.children];
    this.cdk.detectChanges();
  }

  private loadChildren() {
    if (!this.reply) return;
    this.childrenLoading = true;
    this.replyService
      .listReply({
        pageIndex: this.childrenNextPageIndex,
        pageSize: this.childrenPageSize,
        postId: this.reply.postId,
        parentId: this.reply.id,
      })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, pagination }) => {
          this.childrenLoading = false;
          this.children =
            this.children.length === 0 ? result : [...this.children, ...result];
          if (pagination.hasNextPage) this.childrenNextPageIndex++;
          this.childrenHasNextPage = pagination.hasNextPage;
        },
        error: (err) => {
          this.childrenLoading = false;
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }
}
