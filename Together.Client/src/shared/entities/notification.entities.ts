import { IPaginationRequest, IPaginationResult } from '@/core/models';

export interface IListNotificationRequest extends IPaginationRequest {}

export interface IListNotificationResponse
  extends IPaginationResult<INotificationViewModel> {}

export interface INotificationViewModel {
  id: string;
  subId: number;
  createdAt: string;
  actorId: string;
  actorUserName: string;
  actorAvatar?: string;
  activity: string;
  sourceId: string;
}
