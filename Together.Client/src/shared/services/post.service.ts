import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { BehaviorSubject, map, Observable } from 'rxjs';
import {
  ICreatePostRequest,
  IGetPostResponse,
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

  getPost(postId: string): Observable<IGetPostResponse> {
    const url = this.createUrl(postId);
    return this.client
      .get<IBaseResponse<IGetPostResponse>>(url)
      .pipe(map((response) => response.data));
  }

  listPost(params: IListPostRequest): Observable<IListPostResponse> {
    const url = this.createUrl('/list');
    return this.client
      .get<IBaseResponse<IListPostResponse>>(url, {
        params: this.createParams(params),
      })
      .pipe(map((response) => response.data));
  }

  createPost(payload: ICreatePostRequest) {
    const url = this.createUrl('/create');
    return this.client.post(url, payload);
  }
}
