import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import {
  IConversationViewModel,
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
}
