import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { IListPostRequest } from '@/shared/entities/post.entities';
import { AsyncPipe, NgIf } from '@angular/common';
import {
  IPaginatorChange,
  PaginatorComponent,
  PostComponent,
} from '@/shared/components/elements';
import { ActivatedRoute, Router } from '@angular/router';
import { Button } from 'primeng/button';
import { getErrorMessage, windowScrollToTop } from '@/shared/utilities';
import { SkeletonModule } from 'primeng/skeleton';
import { IPagination } from '@/core/models';

@Component({
  selector: 'together-post-list',
  standalone: true,
  imports: [
    AsyncPipe,
    PostComponent,
    Button,
    NgIf,
    SkeletonModule,
    PaginatorComponent,
  ],
  templateUrl: './post-list.component.html',
})
export class PostListComponent extends BaseComponent implements OnInit {
  protected readonly Array = Array;

  params: IListPostRequest = { pageIndex: 1, pageSize: 10 };

  totalRecord = 0;

  loading = false;

  extra: any;

  constructor(
    protected postService: PostService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    super();
  }

  ngOnInit() {
    this.params.pageIndex =
      this.activatedRoute.snapshot.queryParams['pageIndex'] ?? 1;
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
    this.loading = true;
    this.postService
      .listPost(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, pagination, extra }) => {
          this.loading = false;
          this.postService.posts$.next(result);
          this.extra = extra;
          this.totalRecord = pagination.totalRecord;
          this.commonService.breadcrumb$.next([
            {
              title: extra['forumName'],
            },
          ]);
          windowScrollToTop();
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

  onPaginationChange(value: IPaginatorChange) {
    if (value.pageIndex === this.params.pageIndex) return;
    this.params.pageIndex = value.pageIndex;
    this.router
      .navigate([], {
        queryParams: {
          pageIndex: this.params.pageIndex,
        },
        queryParamsHandling: 'merge',
      })
      .then();
    this.loadData();
  }
}
