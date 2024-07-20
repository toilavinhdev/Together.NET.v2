import { Component, ViewChild } from '@angular/core';
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
import { AsyncPipe } from '@angular/common';
import { Menu, MenuModule } from 'primeng/menu';
import { PrimeTemplate } from 'primeng/api';
import { Ripple } from 'primeng/ripple';
import { TranslateModule } from '@ngx-translate/core';

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
  ],
  templateUrl: './notification-dropdown.component.html',
})
export class NotificationDropdownComponent extends BaseComponent {
  @ViewChild('menu', { static: true })
  private menuComponent!: Menu;

  notifications: INotificationViewModel[] = [];

  notificationParams: IListNotificationRequest = {
    pageIndex: 1,
    pageSize: 10,
  };

  constructor(private notificationService: NotificationService) {
    super();
  }

  private loadNotifications() {
    this.notificationService
      .listNotification(this.notificationParams)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result }) => {
          this.notifications = result;
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
    this.loadNotifications();
    this.menuComponent.toggle(event);
  }
}
