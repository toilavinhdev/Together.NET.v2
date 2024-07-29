import { Component } from '@angular/core';
import { NgForOf } from '@angular/common';
import { MenuItem } from 'primeng/api';
import { BaseComponent } from '@/core/abstractions';
import { MenuModule } from 'primeng/menu';
import { Ripple } from 'primeng/ripple';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'together-menu',
  standalone: true,
  imports: [NgForOf, MenuModule, Ripple, TranslateModule],
  templateUrl: './menu.component.html',
})
export class MenuComponent extends BaseComponent {
  items: MenuItem[] = [
    {
      label: 'Trang chủ',
      items: [
        {
          label: 'Thống kê diễn đàn',
          icon: 'pi pi-home',
          routerLink: '/management',
        },

        {
          label: 'Trở lại diễn đàn',
          icon: 'pi pi-angle-left',
          command: () => {
            this.commonService.navigateToMain();
          },
        },
      ],
    },
    {
      label: 'Diễn đàn',
      items: [
        {
          label: 'Danh sách diễn đàn',
          icon: 'pi pi-table',
          routerLink: '/management/forum',
        },
        {
          label: 'Tạo diễn đàn',
          icon: 'pi pi-pencil',
          routerLink: '/management/forum/create',
        },
        {
          label: 'Tạo chủ đề',
          icon: 'pi pi-pencil',
          routerLink: '/management/forum/topic/create',
        },
      ],
    },
    {
      label: 'Prefix',
      items: [
        {
          label: 'Danh sách prefix',
          icon: 'pi pi-table',
          routerLink: '/management/prefix',
        },
        {
          label: 'Tạo prefix',
          icon: 'pi pi-pencil',
          routerLink: '/management/prefix/create',
        },
      ],
    },
    {
      label: 'Bài viết',
      items: [
        {
          label: 'Danh sách bài viết',
          icon: 'pi pi-table',
          routerLink: '/management/post',
        },
        {
          label: 'Báo cáo bài viết',
          icon: 'pi pi-pencil',
          routerLink: '/management/post/reports',
        },
      ],
    },
    {
      label: 'Thành viên',
      items: [
        {
          label: 'Danh sách thành viên',
          icon: 'pi pi-users',
          routerLink: '/management/user',
        },
      ],
    },
    {
      label: 'Vai trò',
      items: [
        {
          label: 'Danh sách vai trò',
          icon: 'pi pi-table',
          routerLink: '/management/role',
        },
        {
          label: 'Tạo vai trò',
          icon: 'pi pi-pencil',
          routerLink: '/management/role/create',
        },
        {
          label: 'Gán vai trò',
          icon: 'pi pi-pencil',
          routerLink: '/management/role/assign',
        },
      ],
    },
  ];
}
