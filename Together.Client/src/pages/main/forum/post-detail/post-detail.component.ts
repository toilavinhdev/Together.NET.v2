import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import { ActivatedRoute } from '@angular/router';
import { Observable, takeUntil } from 'rxjs';
import { IGetPostResponse } from '@/shared/entities/post.entities';
import { getErrorMessage } from '@/shared/utilities';
import { AvatarComponent, PrefixComponent } from '@/shared/components/elements';
import { AsyncPipe, NgIf } from '@angular/common';
import {
  SanitizeHtmlPipe,
  ShortenNumberPipe,
  TimeAgoPipe,
} from '@/shared/pipes';

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
  ],
  templateUrl: './post-detail.component.html',
})
export class PostDetailComponent extends BaseComponent implements OnInit {
  post$ = new Observable<IGetPostResponse | undefined>(undefined);

  constructor(
    private postService: PostService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.post$ = this.postService.post$;
    this.loadData();
  }

  private loadData() {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const postId = paramMap.get('postId');
        if (!postId) return;
        this.postService
          .getPost(postId)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (data) => {
              this.postService.post$.next(data);
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
