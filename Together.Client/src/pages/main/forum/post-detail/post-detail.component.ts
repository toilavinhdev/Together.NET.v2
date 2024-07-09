import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import { ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs';
import { IGetPostResponse } from '@/shared/entities/post.entities';
import { getErrorMessage } from '@/shared/utilities';
import {
  AvatarComponent,
  PrefixComponent,
  ReplyWriterComponent,
  VoteComponent,
} from '@/shared/components/elements';
import { AsyncPipe, NgIf } from '@angular/common';
import {
  SanitizeHtmlPipe,
  ShortenNumberPipe,
  TimeAgoPipe,
} from '@/shared/pipes';
import { ReplyRootListComponent } from '@/pages/main/forum/post-detail/_components';

@Component({
  selector: 'together-post-detail',
  standalone: true,
  imports: [
    PrefixComponent,
    AsyncPipe,
    NgIf,
    AvatarComponent,
    TimeAgoPipe,
    ShortenNumberPipe,
    SanitizeHtmlPipe,
    ReplyWriterComponent,
    ReplyRootListComponent,
    VoteComponent,
  ],
  templateUrl: './post-detail.component.html',
})
export class PostDetailComponent extends BaseComponent implements OnInit {
  post!: IGetPostResponse;

  postId = '';

  constructor(
    private postService: PostService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.loadPost();
  }

  private loadPost() {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const postId = paramMap.get('postId');
        if (!postId) return;
        this.postId = postId;
        this.postService
          .getPost(postId)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (data) => {
              this.post = data;
              this.commonService.breadcrumb$.next([
                {
                  title: data.forumName,
                  routerLink: ['/', 'topics', data.topicId],
                },
                {
                  title: data.topicName,
                },
              ]);
            },
            error: (err) => {
              this.commonService.toast$.next({
                type: 'error',
                message: getErrorMessage(err),
              });
            },
          });
      });
  }
}
