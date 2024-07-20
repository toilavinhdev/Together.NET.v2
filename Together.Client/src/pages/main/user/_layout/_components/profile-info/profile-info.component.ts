import { Component, OnInit } from '@angular/core';
import { AvatarComponent } from '@/shared/components/elements';
import { BaseComponent } from '@/core/abstractions';
import { UserService } from '@/shared/services';
import { takeUntil, tap } from 'rxjs';
import { IGetUserResponse } from '@/shared/entities/user.entities';
import { AsyncPipe, DatePipe, NgIf } from '@angular/common';
import { ShortenNumberPipe } from '@/shared/pipes';
import { ActivatedRoute } from '@angular/router';
import { getErrorMessage } from '@/shared/utilities';
import { SkeletonModule } from 'primeng/skeleton';
import { ProfileUpdateModalComponent } from '../profile-update-modal/profile-update-modal.component';
import { ProfileGetPrivateConversationComponent } from '../profile-get-private-conversation/profile-get-private-conversation.component';

@Component({
  selector: 'together-profile-info',
  standalone: true,
  imports: [
    AvatarComponent,
    AsyncPipe,
    DatePipe,
    NgIf,
    ShortenNumberPipe,
    SkeletonModule,
    ProfileUpdateModalComponent,
    ProfileGetPrivateConversationComponent,
  ],
  templateUrl: './profile-info.component.html',
})
export class ProfileInfoComponent extends BaseComponent implements OnInit {
  user!: IGetUserResponse | undefined;

  loading = false;

  constructor(
    protected userService: UserService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.commonService.breadcrumb$.next([
      {
        title: 'Profile',
      },
    ]);
    this.userService.user$
      .pipe(takeUntil(this.destroy$))
      .subscribe((user) => (this.user = user));
    this.loadData();
  }

  private loadData() {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const userId = paramMap.get('userId');
        if (!userId) return;
        this.userService
          .getUser(userId)
          .pipe(
            takeUntil(this.destroy$),
            tap(() => {
              this.loading = true;
            }),
          )
          .subscribe({
            next: (data) => {
              this.loading = false;
              this.userService.user$.next(data);
            },
            error: (err) => {
              this.loading = false;
              this.commonService.toast$.next({
                type: 'error',
                message: getErrorMessage(err),
              });
            },
          });
      });
  }
}
