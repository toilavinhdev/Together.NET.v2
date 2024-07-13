import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import {
  IListPostRequest,
  IPostViewModel,
} from '@/shared/entities/post.entities';
import { ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { PostComponent } from '@/shared/components/elements';

@Component({
  selector: 'together-profile-post-list',
  standalone: true,
  imports: [PostComponent],
  templateUrl: './profile-post-list.component.html',
})
export class ProfilePostListComponent extends BaseComponent implements OnInit {
  posts: IPostViewModel[] = [];

  params: IListPostRequest = {
    userId: '',
    pageIndex: 1,
    pageSize: 8,
  };

  constructor(
    private postService: PostService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const userId = paramMap.get('userId');
        if (!userId) return;
        this.params.userId = userId;
        this.loadPosts();
      });
  }

  private loadPosts() {
    this.postService
      .listPost(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result }) => {
          this.posts = result;
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
