import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { IToast } from '@/shared/models/toast.models';
import { IBreadcrumbItem } from '@/shared/models/breadcrumb.models';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  spinning$ = new Subject<boolean>();

  toast$ = new BehaviorSubject<IToast | undefined>(undefined);

  breadcrumb$ = new BehaviorSubject<IBreadcrumbItem[]>([]);

  constructor(private router: Router) {}

  navigateToLogin() {
    this.router.navigate(['/', 'auth', 'sign-in']).then();
  }

  navigateToMain() {
    this.router.navigate(['/']).then();
  }

  navigateToTopic(topicId: string) {
    this.router.navigate(['/', 'topics', topicId]).then();
  }

  navigateToCreatePost(topicId?: string) {
    this.router.navigate(['/', 'topics', topicId ?? 0, 'create-post']).then();
  }

  navigateToPost(postId: string) {
    this.router.navigate(['/', 'posts', postId]).then();
  }

  navigateToUpdatePost(postId: string) {
    this.router.navigate(['/', 'posts', postId, 'update-post']).then();
  }

  navigateToProfile(userId: string) {
    this.router.navigate(['/', 'user', userId]).then();
  }

  navigateToConversation(conversationId: string) {
    this.router.navigate(['/', 'conversations', conversationId]).then();
  }
}
