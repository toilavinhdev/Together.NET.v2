import { Component, ViewChild } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { AvatarComponent } from '@/shared/components/elements';
import { Menu, MenuModule } from 'primeng/menu';
import { MenuItem, PrimeTemplate } from 'primeng/api';
import { Ripple } from 'primeng/ripple';
import { takeUntil } from 'rxjs';
import { BaseComponent } from '@/core/abstractions';
import { AuthService, UserService } from '@/shared/services';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-user-dropdown',
  standalone: true,
  imports: [AsyncPipe, AvatarComponent, MenuModule, PrimeTemplate, Ripple],
  templateUrl: './user-dropdown.component.html',
})
export class UserDropdownComponent extends BaseComponent {
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

  @ViewChild('menu', { static: true })
  menuComponent!: Menu;

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
