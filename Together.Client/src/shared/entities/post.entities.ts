import { IPaginationRequest, IPaginationResult } from '@/core/models';

export interface IListPostRequest extends IPaginationRequest {
  topicId?: string;
  userId?: string;
}

export interface IListPostResponse extends IPaginationResult<IPostViewModel> {
  extra: any;
}

export interface IPostViewModel {
  id: string;
  subId: number;
  forumId: string;
  topicId: string;
  topicName: string;
  prefixId?: string;
  prefixName?: string;
  prefixForeground?: string;
  prefixBackground?: string;
  title: string;
  body: string;
  createdAt: string;
  modifiedAt: string;
  createdById: string;
  createdByUserName: string;
  createdByAvatar?: string;
  replyCount: number;
  postCount: number;
}
