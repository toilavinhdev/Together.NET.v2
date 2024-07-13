import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import {
  IConversationViewModel,
  ICreateConversationRequest,
  ICreateConversationResponse,
  IGetConversationRequest,
  IListConversationRequest,
  IListConversationResponse,
} from '@/shared/entities/conversation.entities';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class ConversationService extends BaseService {
  conversations$ = new BehaviorSubject<IConversationViewModel[]>([]);

  constructor() {
    super();
    this.setEndpoint('/conversation');
  }

  getConversation(
    params: IGetConversationRequest,
  ): Observable<IConversationViewModel | undefined> {
    const url = this.createUrl('/get');
    return this.client
      .get<
        IBaseResponse<IConversationViewModel | undefined>
      >(url, { params: this.createParams(params) })
      .pipe(map((response) => response.data));
  }

  listConversation(
    params: IListConversationRequest,
  ): Observable<IListConversationResponse> {
    const url = this.createUrl('/list');
    return this.client
      .get<
        IBaseResponse<IListConversationResponse>
      >(url, { params: this.createParams(params) })
      .pipe(map((response) => response.data));
  }

  createConversation(
    payload: ICreateConversationRequest,
  ): Observable<ICreateConversationResponse> {
    const url = this.createUrl('/create');
    return this.client
      .post<IBaseResponse<ICreateConversationResponse>>(url, payload)
      .pipe(map((response) => response.data));
  }
}
