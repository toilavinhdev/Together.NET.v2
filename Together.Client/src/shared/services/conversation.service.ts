import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import {
  IConversationViewModel,
  IConversationQueryRequest,
  IConversationQueryResponse,
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

  queryConversation(
    params: IConversationQueryRequest,
  ): Observable<IConversationQueryResponse> {
    const url = this.createUrl('/query');
    return this.client
      .get<
        IBaseResponse<IConversationQueryResponse>
      >(url, { params: this.createParams(params) })
      .pipe(map((response) => response.data));
  }
}
