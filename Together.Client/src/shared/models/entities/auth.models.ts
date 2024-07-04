import { ITokenModel } from '@/core/models';
import { EGender } from '@/shared/enums';

export interface ICurrentUserClaims {
  id: string;
  subId: string;
  userName: string;
  email: string;
  nbf: number;
  iat: number;
  exp: number;
  iss: string;
  aud: string;
}

export interface ISignInRequest {
  email: string;
  password: string;
}

export interface ISignInResponse extends ITokenModel {}

export interface ISignUpRequest {
  userName: string;
  email: string;
  gender: EGender;
  password: string;
  confirmPassword: string;
}

export interface IExternalAuthRequest {
  credential: string;
}
