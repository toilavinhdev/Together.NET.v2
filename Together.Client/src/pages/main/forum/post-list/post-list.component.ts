import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { IListPostRequest } from '@/shared/entities/post.entities';
import { AsyncPipe, NgIf } from '@angular/common';
import { PostComponent } from '@/shared/components/elements';
import { ActivatedRoute } from '@angular/router';
import { Button } from 'primeng/button';
import { getErrorMessage } from '@/shared/utilities';
import { SkeletonModule } from 'primeng/skeleton';

@Component({
  selector: 'together-post-list',
  standalone: true,
  imports: [AsyncPipe, PostComponent, Button, NgIf, SkeletonModule],
  templateUrl: './post-list.component.html',
})
export class PostListComponent extends BaseComponent implements OnInit {
  protected readonly Array = Array;

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
    this.loading = true;
    this.postService
      .listPost(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, extra }) => {
          this.loading = false;
          this.postService.posts$.next(result);
          this.extra = extra;
          this.commonService.breadcrumb$.next([
            {
              title: extra['forumName'],
            },
          ]);
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
