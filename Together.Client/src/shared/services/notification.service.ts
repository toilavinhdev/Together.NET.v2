import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import {
  IListNotificationRequest,
  IListNotificationResponse,
} from '@/shared/entities/notification.entities';
import { IBaseResponse } from '@/core/models';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationService extends BaseService {
  constructor() {
    super();
    this.setEndpoint('/notification');
  }

  listNotification(params: IListNotificationRequest) {
    const url = this.createUrl('/list');
    return this.client
      .get<
        IBaseResponse<IListNotificationResponse>
      >(url, { params: this.createParams(params) })
      .pipe(map((response) => response.data));
  }
}
