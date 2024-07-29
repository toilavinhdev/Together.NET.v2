import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { UserService } from '@/shared/services';
import {
  IListUserRequest,
  IUserViewModel,
} from '@/shared/entities/user.entities';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import {
  AvatarComponent,
  TableCellDirective,
  TableColumnDirective,
  TableComponent,
  UserStatusComponent,
} from '@/shared/components/elements';
import { Button } from 'primeng/button';

@Component({
  selector: 'together-m-user-list',
  standalone: true,
  imports: [
    TableComponent,
    Button,
    TableCellDirective,
    TableColumnDirective,
    AvatarComponent,
    UserStatusComponent,
  ],
  templateUrl: './m-user-list.component.html',
})
export class MUserListComponent extends BaseComponent implements OnInit {
  users: IUserViewModel[] = [];

  params: IListUserRequest = {
    pageIndex: 1,
    pageSize: 12,
    search: undefined,
  };

  totalUsers = 0;

  constructor(private userService: UserService) {
    super();
  }

  ngOnInit() {
    this.commonService.title$.next('Danh sách thành viên');
    this.loadUsers();
  }

  loadUsers() {
    this.userService
      .listUser(this.params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result, pagination }) => {
          this.users = result;
          this.totalUsers = pagination.totalRecord;
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }
}
