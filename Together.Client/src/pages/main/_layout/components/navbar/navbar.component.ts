import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AvatarComponent,
  SvgIconComponent,
} from '@/shared/components/elements';
import { Button } from 'primeng/button';
import { BaseComponent } from '@/core/abstractions';
import { UserService } from '@/shared/services';
import { AsyncPipe } from '@angular/common';
import { Ripple } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';

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
      items: [
        {
          label: 'Refresh',
          icon: 'pi pi-refresh',
          command: () => {
            console.log(111);
          },
        },
        {
          label: 'Export',
          icon: 'pi pi-upload',
          command: () => {
            console.log(222);
          },
        },
      ],
    },
  ];

  constructor(protected userService: UserService) {
    super();
  }
}
