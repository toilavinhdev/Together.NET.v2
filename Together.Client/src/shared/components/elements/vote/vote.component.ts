import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgClass } from '@angular/common';
import { BaseComponent } from '@/core/abstractions';
import { EVoteType } from '@/shared/enums';
import { ShortenNumberPipe } from '@/shared/pipes';
import { PostService, ReplyService } from '@/shared/services';
import { Observable, takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { IVoteResponse } from '@/shared/entities/post.entities';

@Component({
  selector: 'together-vote',
  standalone: true,
  imports: [NgClass, ShortenNumberPipe],
  templateUrl: './vote.component.html',
})
export class VoteComponent extends BaseComponent {
  @Input()
  sourceId = '';

  @Input()
  voteFor: 'post' | 'reply' = 'post';

  @Input()
  voteUpCount = 0;

  @Input()
  voteDownCount = 0;

  @Input()
  replyCount = 0;

  @Input()
  voted?: EVoteType;

  @Output()
  expandReplyWriter = new EventEmitter<void>();

  @Output()
  voteResponse = new EventEmitter<IVoteResponse>();

  constructor(
    private postService: PostService,
    private replyService: ReplyService,
  ) {
    super();
  }

  onVote(type: EVoteType) {
    if (!this.sourceId) return;
    this.getObservable(type)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.voteResponse.emit(data);
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  private getObservable(type: EVoteType): Observable<IVoteResponse> {
    if (this.voteFor === 'post') {
      return this.postService.votePost({
        postId: this.sourceId,
        type: type,
      });
    } else if (this.voteFor === 'reply') {
      return this.replyService.voteReply({
        replyId: this.sourceId,
        type: type,
      });
    }
    return new Observable<IVoteResponse>();
  }

  protected readonly EVoteType = EVoteType;
}
