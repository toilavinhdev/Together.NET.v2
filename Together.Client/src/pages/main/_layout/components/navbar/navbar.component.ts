import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AvatarComponent,
  NotificationDropdownComponent,
  SvgIconComponent,
  UserDropdownComponent,
} from '@/shared/components/elements';
import { BaseComponent } from '@/core/abstractions';
import { UserService } from '@/shared/services';
import { AsyncPipe } from '@angular/common';

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
