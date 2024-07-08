import { EGender, EUserStatus } from '@/shared/enums';

export interface IMeResponse {
  id: string;
  subId: number;
  userName: string;
  email: string;
  status: EUserStatus;
  avatar?: string;
}

export interface IGetUserResponse {
  id: string;
  subId: number;
  createdAt: string;
  userName: string;
  gender: EGender;
  fullName?: string;
  avatar?: string;
  biography?: string;
  postCount: number;
  replyCount: number;
}

export interface IUpdateProfileRequest {
  userName: string;
  gender: EGender;
  fullName?: string;
  biography?: string;
}

export interface IUpdatePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}
