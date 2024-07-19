import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AvatarComponent,
  SvgIconComponent,
} from '@/shared/components/elements';
import { BaseComponent } from '@/core/abstractions';
import { UserService } from '@/shared/services';
import { AsyncPipe } from '@angular/common';
import { UserDropdownComponent } from './_components/user-dropdown/user-dropdown.component';
import { NotificationDropdownComponent } from './_components/notification-dropdown/notification-dropdown.component';

@Component({
  selector: 'together-navbar',
  standalone: true,
  imports: [
    RouterLink,
    SvgIconComponent,
    AvatarComponent,
    AsyncPipe,
    UserDropdownComponent,
    NotificationDropdownComponent,
  ],
  templateUrl: './navbar.component.html',
})
export class NavbarComponent extends BaseComponent {
  constructor(protected userService: UserService) {
    super();
  }
}
