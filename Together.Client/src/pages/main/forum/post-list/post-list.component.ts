import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { IListPostRequest } from '@/shared/entities/post.entities';
import { AsyncPipe } from '@angular/common';
import { PostComponent } from '@/shared/components/elements';
import { ActivatedRoute } from '@angular/router';
import { Button } from 'primeng/button';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-post-list',
  standalone: true,
  imports: [AsyncPipe, PostComponent, Button],
  templateUrl: './post-list.component.html',
})
export class PostListComponent extends BaseComponent implements OnInit {
  params: IListPostRequest = { pageIndex: 1, pageSize: 12 };

  loading = false;

  extra: any;

  constructor(
    protected postService: PostService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const topicId = paramMap.get('topicId');
        if (!topicId) return;
        this.params.topicId = topicId;
        this.loadData();
      });
  }

  override ngOnDestroy() {
    super.ngOnDestroy();
    this.postService.posts$.next([]);
  }

  private loadData() {
    this.postService
      .listPost(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, extra }) => {
          this.postService.posts$.next(result);
          this.extra = extra;
        },
        error: (err) => {
          this.loading = false;
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }
}
