import { IPaginationRequest, IPaginationResult } from '@/core/models';

export interface IListConversationRequest extends IPaginationRequest {}

export interface IListConversationResponse
  extends IPaginationResult<IConversationViewModel> {}

export interface IConversationViewModel {
  id: string;
  subId: number;
  name: string;
  image?: string;
  lastMessageByUserId?: string;
  lastMessageByUserName?: string;
  lastMessageText?: string;
  lastMessageAt?: string;
}
