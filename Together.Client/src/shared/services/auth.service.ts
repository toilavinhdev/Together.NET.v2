import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import {
  ISignInRequest,
  ISignInResponse,
  ISignUpRequest,
} from '@/shared/models/entities/auth.models';
import { map, Observable } from 'rxjs';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseService {
  constructor() {
    super();
    this.setEndpoint('/auth');
  }

  signIn(payload: ISignInRequest): Observable<ISignInResponse> {
    const url = this.createUrl('/sign-in');
    return this.client
      .post<IBaseResponse<ISignInResponse>>(url, payload)
      .pipe(map((r) => r.data));
  }

  signUp(payload: ISignUpRequest) {
    const url = this.createUrl('/sign-up');
    return this.client.post(url, payload);
  }

  logout() {
    const url = this.createUrl('/logout');
    return this.client.post(url, {});
  }
}
