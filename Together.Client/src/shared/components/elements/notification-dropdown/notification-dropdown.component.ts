import { Component, OnInit, ViewChild } from '@angular/core';
import { AvatarComponent } from '@/shared/components/elements';
import { TimeAgoPipe } from '@/shared/pipes';
import {
  IListNotificationRequest,
  INotificationViewModel,
} from '@/shared/entities/notification.entities';
import { notificationActivity } from '@/shared/constants';
import { BaseComponent } from '@/core/abstractions';
import { takeUntil } from 'rxjs';
import { NotificationService } from '@/shared/services';
import { AsyncPipe, NgForOf } from '@angular/common';
import { Menu, MenuModule } from 'primeng/menu';
import { PrimeTemplate } from 'primeng/api';
import { Ripple } from 'primeng/ripple';
import { TranslateModule } from '@ngx-translate/core';
import { getErrorMessage } from '@/shared/utilities';
import { SkeletonModule } from 'primeng/skeleton';

@Component({
  selector: 'together-notification-dropdown',
  standalone: true,
  imports: [
    AvatarComponent,
    TimeAgoPipe,
    AsyncPipe,
    MenuModule,
    PrimeTemplate,
    Ripple,
    TranslateModule,
    SkeletonModule,
    NgForOf,
  ],
  templateUrl: './notification-dropdown.component.html',
})
export class NotificationDropdownComponent
  extends BaseComponent
  implements OnInit
{
  @ViewChild('menu', { static: true })
  private menuComponent!: Menu;

  notifications: INotificationViewModel[] = [];

  notificationParams: IListNotificationRequest = {
    pageIndex: 1,
    pageSize: 6,
  };

  status: 'idle' | 'loading ' | 'finished' = 'idle';

  constructor(private notificationService: NotificationService) {
    super();
  }

  ngOnInit() {}

  private loadNotifications() {
    this.status = 'loading ';
    this.notificationService
      .listNotification(this.notificationParams)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result }) => {
          this.notifications = result;
          this.status = 'finished';
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  navigateToSource(srcId: string, activity: string) {
    if (activity === notificationActivity.VOTE_POST) {
      this.commonService.navigateToPost(srcId);
    }
    this.menuComponent.hide();
  }

  toggle(event: Event) {
    if (this.status !== 'finished') {
      this.loadNotifications();
    }
    this.menuComponent.toggle(event);
  }

  protected readonly Array = Array;
}
