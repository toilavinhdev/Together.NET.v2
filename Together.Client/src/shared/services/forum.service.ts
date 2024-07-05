import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { IForumViewModel } from '@/shared/entities/forum.entities';
import { IBaseResponse } from '@/core/models';
import { BaseService } from '@/core/abstractions';

@Injectable({
  providedIn: 'root',
})
export class ForumService extends BaseService {
  forums$ = new BehaviorSubject<IForumViewModel[]>([]);

  constructor() {
    super();
    this.setEndpoint('/forum');
  }

  listForum(): Observable<IForumViewModel[]> {
    const url = this.createUrl('/list');
    return this.client
      .get<IBaseResponse<IForumViewModel[]>>(url)
      .pipe(map((response) => response.data));
  }
}
