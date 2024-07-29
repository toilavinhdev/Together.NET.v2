import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgClass } from '@angular/common';
import { BaseComponent } from '@/core/abstractions';
import { EVoteType } from '@/shared/enums';
import { ShortenNumberPipe } from '@/shared/pipes';
import { PostService, ReplyService } from '@/shared/services';
import { Observable, takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { IVoteResponse } from '@/shared/entities/post.entities';
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';
import { Ripple } from 'primeng/ripple';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'together-vote',
  standalone: true,
  imports: [NgClass, ShortenNumberPipe, MenuModule, Ripple, TranslateModule],
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

  items: MenuItem[] = [
    {
      label: this.voteFor === 'post' ? 'Update post' : 'Update reply',
      icon: 'pi pi-pencil',
    },
    {
      label: this.voteFor === 'post' ? 'Delete post' : 'Delete reply',
      icon: 'pi pi-trash',
    },
    {
      label: 'Report',
      icon: 'pi pi-flag',
    },
  ];

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
