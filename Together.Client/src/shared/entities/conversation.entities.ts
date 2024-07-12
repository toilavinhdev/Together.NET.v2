import { IPaginationRequest, IPaginationResult } from '@/core/models';

export interface IConversationQueryRequest extends IPaginationRequest {
  conversationId?: string;
}

export interface IConversationQueryResponse
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
