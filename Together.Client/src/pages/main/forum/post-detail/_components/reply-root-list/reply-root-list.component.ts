import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ReplyService } from '@/shared/services';
import {
  IListReplyRequest,
  IReplyViewModel,
} from '@/shared/entities/reply.entities';
import { ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { ReplyComponent } from '@/shared/components/elements';

@Component({
  selector: 'together-reply-root-list',
  standalone: true,
  imports: [ReplyComponent],
  templateUrl: './reply-root-list.component.html',
})
export class ReplyRootListComponent extends BaseComponent implements OnInit {
  replies: IReplyViewModel[] = [];

  params: IListReplyRequest = {
    pageIndex: 1,
    pageSize: 14,
  };

  loading = false;

  constructor(
    private replyService: ReplyService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const postId = paramMap.get('postId');
        if (!postId) return;
        this.params.postId = postId;
        this.loadRootReplies();
      });
  }

  private loadRootReplies() {
    this.loading = true;
    this.replyService
      .listReply(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result }) => {
          this.loading = true;
          this.replies = result;
        },
        error: (err) => {
          this.loading = true;
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  addRootReply(reply: IReplyViewModel) {
    this.replies = [reply, ...this.replies];
  }
}
