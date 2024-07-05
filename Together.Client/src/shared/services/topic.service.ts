import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';

@Injectable({
  providedIn: 'root',
})
export class TopicService extends BaseService {
  constructor() {
    super();
    this.setEndpoint('/topic');
  }
}
