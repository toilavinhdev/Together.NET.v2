import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ForumService } from '@/shared/services/forum.service';
import { Observable, takeUntil } from 'rxjs';
import { IForumViewModel } from '@/shared/entities/forum.entities';
import { AsyncPipe, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ShortenNumberPipe } from '@/shared/pipes';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-topic-list',
  standalone: true,
  imports: [AsyncPipe, RouterLink, NgIf, ShortenNumberPipe],
  templateUrl: './topic-list.component.html',
})
export class TopicListComponent extends BaseComponent implements OnInit {
  forums$ = new Observable<IForumViewModel[]>();

  constructor(protected forumService: ForumService) {
    super();
  }

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.forumService
      .listForum()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.forumService.forums$.next(data);
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  protected readonly top = top;
}
