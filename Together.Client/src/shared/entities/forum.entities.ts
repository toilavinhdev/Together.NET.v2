import { ITopicViewModel } from '@/shared/entities/topic.entities';

export interface IForumViewModel {
  id: string;
  subId: number;
  name: string;
  topics?: ITopicViewModel[];
}
