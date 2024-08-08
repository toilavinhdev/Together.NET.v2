import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';
import {
  IGetUserResponse,
  IListUserRequest,
  IListUserResponse,
  IMeResponse,
  IUpdatePasswordRequest,
  IUpdateProfileRequest,
} from '@/shared/entities/user.entities';
import { IBaseResponse } from '@/core/models';
import { policies } from '@/shared/constants';

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

  hasPermission$ = (policy: string): Observable<boolean> =>
    this.me$.pipe(
      map((me) =>
        !me ? false : me.permissions.includes(policies.All || policy),
      ),
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

  listUser(params: IListUserRequest): Observable<IListUserResponse> {
    const url = this.createUrl('list');
    return this.client
      .get<
        IBaseResponse<IListUserResponse>
      >(url, { params: this.createParams(params) })
      .pipe(map((response) => response.data));
  }

  updateProfile(payload: IUpdateProfileRequest) {
    const url = this.createUrl('/me/update-profile');
    return this.client.put(url, payload);
  }

  updatePassword(payload: IUpdatePasswordRequest) {
    const url = this.createUrl('/me/update-password');
    return this.client.put(url, payload);
  }
}
