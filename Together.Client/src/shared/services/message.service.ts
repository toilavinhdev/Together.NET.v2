import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import {
  IListMessageRequest,
  IListMessageResponse,
  ISendMessageRequest,
  ISendMessageResponse,
} from '@/shared/entities/message.entities';
import { map, Observable } from 'rxjs';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class MessageService extends BaseService {
  constructor() {
    super();
    this.setEndpoint('/message');
  }

  listMessage(params: IListMessageRequest): Observable<IListMessageResponse> {
    const url = this.createUrl('/list');
    return this.client
      .get<
        IBaseResponse<IListMessageResponse>
      >(url, { params: this.createParams(params) })
      .pipe(map((response) => response.data));
  }

  sendMessage(payload: ISendMessageRequest): Observable<ISendMessageResponse> {
    const url = this.createUrl('/send');
    return this.client
      .post<IBaseResponse<ISendMessageResponse>>(url, payload)
      .pipe(map((response) => response.data));
  }
}
