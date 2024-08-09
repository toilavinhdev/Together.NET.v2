import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MenuComponent } from '@/pages/management/_layout/_components/menu/menu.component';
import { HeaderComponent } from '@/pages/management/_layout/_components/header/header.component';
import { UserService } from '@/shared/services';
import { AsyncPipe, NgIf } from '@angular/common';
import { policies } from '@/shared/constants';
import { BaseComponent } from '@/core/abstractions';
import { take, takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { Button } from 'primeng/button';

@Component({
  selector: 'together-management',
  standalone: true,
  imports: [
    RouterOutlet,
    MenuComponent,
    HeaderComponent,
    NgIf,
    AsyncPipe,
    Button,
    RouterLink,
  ],
  templateUrl: './management.component.html',
})
export class ManagementComponent extends BaseComponent implements OnInit {
  constructor(protected userService: UserService) {
    super();
  }

  protected readonly policies = policies;

  status: 'idle' | 'loading' | 'finished' = 'idle';

  ngOnInit() {
    this.loadMe();
  }

  loadMe() {
    this.userService.me$.pipe(take(1)).subscribe((me) => {
      if (!me) {
        this.status = 'loading';
        this.commonService.spinning$.next(true);
        this.userService
          .getMe()
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (me) => {
              this.status = 'finished';
              this.commonService.spinning$.next(false);
              this.userService.me$.next(me);
            },
            error: (err) => {
              this.status = 'finished';
              this.commonService.spinning$.next(false);
              this.commonService.toast$.next({
                type: 'error',
                message: getErrorMessage(err),
              });
            },
          });
      } else {
        this.status = 'finished';
      }
    });
  }
}
