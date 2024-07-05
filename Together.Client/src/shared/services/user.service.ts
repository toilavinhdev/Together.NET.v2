import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';
import { IGetUserResponse, IMeResponse } from '@/shared/entities/user.entities';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseService {
  me$ = new BehaviorSubject<IMeResponse | undefined>(undefined);

  user$ = new BehaviorSubject<IGetUserResponse | undefined>(undefined);

  userIsMe$ = combineLatest([this.me$, this.user$]).pipe(
    map(([me, user]) => {
      if (!me || !user) return false;
      return me.id === user.id;
    }),
  );

  constructor() {
    super();
    this.setEndpoint('/user');
  }

  getMe(): Observable<IMeResponse> {
    const url = this.createUrl('/me');
    return this.client
      .get<IBaseResponse<IMeResponse>>(url)
      .pipe(map((response) => response.data));
  }

  getUser(userId: string): Observable<IGetUserResponse> {
    const url = this.createUrl(userId);
    return this.client
      .get<IBaseResponse<IGetUserResponse>>(url)
      .pipe(map((response) => response.data));
  }
}
