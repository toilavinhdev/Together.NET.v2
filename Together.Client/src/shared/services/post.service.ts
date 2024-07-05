import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { BehaviorSubject, map, Observable } from 'rxjs';
import {
  IListPostRequest,
  IListPostResponse,
  IPostViewModel,
} from '@/shared/entities/post.entities';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class PostService extends BaseService {
  posts$ = new BehaviorSubject<IPostViewModel[]>([]);

  constructor() {
    super();
    this.setEndpoint('/post');
  }

  listPost(params: IListPostRequest): Observable<IListPostResponse> {
    const url = this.createUrl('/list');
    return this.client
      .get<IBaseResponse<IListPostResponse>>(url, {
        params: this.createParams(params),
      })
      .pipe(map((response) => response.data));
  }
}
