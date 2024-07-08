import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AvatarComponent,
  SvgIconComponent,
} from '@/shared/components/elements';
import { Button } from 'primeng/button';
import { BaseComponent } from '@/core/abstractions';
import { AuthService, UserService } from '@/shared/services';
import { AsyncPipe } from '@angular/common';
import { Ripple } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-navbar',
  standalone: true,
  imports: [
    RouterLink,
    SvgIconComponent,
    Button,
    AvatarComponent,
    AsyncPipe,
    Ripple,
    MenuModule,
  ],
  templateUrl: './navbar.component.html',
})
export class NavbarComponent extends BaseComponent {
  items: MenuItem[] = [
    {
      separator: true,
    },
    {
      label: 'Trang cá nhân',
      icon: 'pi pi-user',
      command: () => {
        this.userService.me$.pipe(takeUntil(this.destroy$)).subscribe((me) => {
          if (!me) return;
          this.commonService.navigateToProfile(me.id);
        });
      },
    },
    {
      label: 'Cài đặt',
      icon: 'pi pi-cog',
      routerLink: '/settings',
    },
    {
      label: 'Đăng xuất',
      icon: 'pi pi-sign-out',
      command: () => {
        this.onLogout();
      },
    },
  ];

  constructor(
    protected userService: UserService,
    private authService: AuthService,
  ) {
    super();
  }

  private onLogout() {
    this.commonService.spinning$.next(true);
    this.authService
      .logout()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.commonService.spinning$.next(false);
          this.authService.removeToken();
          localStorage.clear();
          this.commonService.navigateToLogin();
        },
        error: (err) => {
          this.commonService.spinning$.next(false);
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }
}
