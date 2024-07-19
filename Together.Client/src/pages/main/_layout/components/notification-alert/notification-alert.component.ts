import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { WebSocketService } from '@/shared/services';
import { filter, takeUntil } from 'rxjs';
import {
  notificationActivity,
  websocketClientTarget,
} from '@/shared/constants';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { JsonPipe } from '@angular/common';
import { AvatarComponent } from '@/shared/components/elements';
import { TimeAgoPipe } from '@/shared/pipes';

@Component({
  selector: 'together-notification-alert',
  standalone: true,
  imports: [ToastModule, JsonPipe, AvatarComponent, TimeAgoPipe],
  templateUrl: './notification-alert.component.html',
  providers: [MessageService],
})
export class NotificationAlertComponent
  extends BaseComponent
  implements OnInit
{
  constructor(
    private webSocketService: WebSocketService,
    private messageService: MessageService,
  ) {
    super();
  }

  id = 0;

  ngOnInit() {
    this.listenWebSocket();
  }

  private listenWebSocket() {
    this.webSocketService.client$
      .pipe(
        takeUntil(this.destroy$),
        filter(
          (socket) =>
            socket.target === websocketClientTarget.ReceivedNotification,
        ),
      )
      .subscribe({
        next: (socket) => {
          console.log(socket);
          this.messageService.add({
            key: this.id.toString(),
            life: 4000,
            severity: 'secondary',
            closable: true,
            data: socket.message,
          });
          this.id++;
        },
      });
  }

  navigateToSource(key: string, srcId: string, activity: string) {
    if (activity === notificationActivity.VOTE_POST) {
      this.commonService.navigateToPost(srcId);
    }
    this.messageService.clear(key);
  }
}
