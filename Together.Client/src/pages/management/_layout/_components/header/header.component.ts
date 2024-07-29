import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import {
  AvatarComponent,
  NotificationDropdownComponent,
  UserDropdownComponent,
} from '@/shared/components/elements';
import { AsyncPipe } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { UserService } from '@/shared/services';
import { take, takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { Button } from 'primeng/button';

@Component({
  selector: 'together-header',
  standalone: true,
  imports: [
    AvatarComponent,
    UserDropdownComponent,
    AsyncPipe,
    TranslateModule,
    Button,
    NotificationDropdownComponent,
  ],
  templateUrl: './header.component.html',
})
export class HeaderComponent extends BaseComponent implements OnInit {
  constructor(protected userService: UserService) {
    super();
  }

  ngOnInit() {
    this.loadMe();
  }

  loadMe() {
    this.userService.me$.pipe(take(1)).subscribe((me) => {
      if (!me) {
        this.userService
          .getMe()
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (me) => {
              this.userService.me$.next(me);
            },
            error: (err) => {
              this.commonService.toast$.next({
                type: 'error',
                message: getErrorMessage(err),
              });
            },
          });
      }
    });
  }
}
