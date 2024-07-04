import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { IToast } from '@/shared/models/components/toast.models';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  spinning$ = new Subject<boolean>();

  toast$ = new BehaviorSubject<IToast | undefined>(undefined);

  constructor(private router: Router) {}

  navigateToLogin() {
    this.router.navigate(['/', 'auth', 'sign-in']).then();
  }

  navigateToMain() {
    this.router.navigate(['/']).then();
  }
}
