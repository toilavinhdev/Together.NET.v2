import { Component, OnInit } from '@angular/core';
import { DragDropModule } from 'primeng/dragdrop';
import { NgForOf } from '@angular/common';
import { BaseComponent } from '@/core/abstractions';
import {
  IListRoleRequest,
  IListRoleResponse,
  IRoleViewModel,
} from '@/shared/entities/role.entities';
import { RoleService, UserService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import {
  AvatarComponent,
  ContainerComponent,
  TableCellDirective,
  TableColumnDirective,
  TableComponent,
} from '@/shared/components/elements';
import {
  IListUserRequest,
  IUserViewModel,
} from '@/shared/entities/user.entities';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { Button } from 'primeng/button';
import { RouterLink } from '@angular/router';
import { areArraysEqual, getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-m-role-assign',
  standalone: true,
  imports: [
    DragDropModule,
    NgForOf,
    TableComponent,
    ContainerComponent,
    TableColumnDirective,
    DropdownModule,
    FormsModule,
    TableCellDirective,
    AvatarComponent,
    Button,
    RouterLink,
  ],
  templateUrl: './m-role-assign.component.html',
})
export class MRoleAssignComponent extends BaseComponent implements OnInit {
  availableRoles: IRoleViewModel[] = [];

  userRoles: IRoleViewModel[] = [];

  private originalUserRoles: IRoleViewModel[] = [];

  users: IUserViewModel[] = [];

  availableRolesParams: IListRoleRequest = {
    pageIndex: 1,
    pageSize: 12,
  };

  userRolesParams: IListRoleRequest = {
    pageIndex: 1,
    pageSize: 12,
    userId: undefined,
  };

  usersParams: IListUserRequest = {
    pageIndex: 1,
    pageSize: 10,
  };

  rowDraggableDroppable = 'draggingRole';

  draggingRole?: IRoleViewModel;

  get userRolesHasChanged(): boolean {
    return !areArraysEqual(this.userRoles, this.originalUserRoles);
  }

  constructor(
    private roleService: RoleService,
    private userService: UserService,
  ) {
    super();
  }

  ngOnInit() {
    this.commonService.title$.next('Gán vai trò');

    this.loadAvailableRoles();
    this.loadUsers();
  }

  loadAvailableRoles() {
    this.loadRole(this.availableRolesParams, ({ result, pagination }) => {
      this.availableRoles = result;
    });
  }

  loadUserRoles() {
    this.loadRole(this.userRolesParams, ({ result, pagination }) => {
      this.userRoles = result;
      this.originalUserRoles = [...this.userRoles];
      this.updateAvailableRoles();
    });
  }

  private loadRole(
    params: IListRoleRequest,
    callBack: (response: IListRoleResponse) => void,
  ) {
    this.roleService
      .listRole(params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          callBack(response);
        },
        error: (err) => {},
      });
  }

  private loadUsers() {
    this.userService
      .listUser(this.usersParams)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: ({ result }) => {
          this.users = result;
        },
        error: (err) => {},
      });
  }

  onUserIdChange(userId: string) {
    this.userRolesParams.pageIndex = 1;
    this.userRolesParams.userId = userId;
    this.availableRolesParams.pageIndex = 1;
    this.loadAvailableRoles();
    this.loadUserRoles();
  }

  dragStart(role: IRoleViewModel) {
    console.log(role);
    this.draggingRole = role;
  }

  dragEnd() {
    if (!this.userRolesParams.userId) return;
    this.draggingRole = undefined;
  }

  drop() {
    if (!this.userRolesParams.userId) {
      this.commonService.toast$.next({
        type: 'error',
        message: 'Vui lòng chọn thành viên',
      });
      return;
    }
    this.availableRoles = this.availableRoles.filter(
      (r) => r.id !== this.draggingRole?.id,
    );
    this.userRoles = [...this.userRoles, this.draggingRole!];
  }

  private updateAvailableRoles() {
    this.availableRoles = this.availableRoles.filter(
      (availableRole) =>
        !this.userRoles.some((userRole) => userRole.id === availableRole.id),
    );
  }

  removeUserRole(role: IRoleViewModel) {
    this.userRoles = this.userRoles.filter(
      (userRole) => userRole.id !== role.id,
    );
    this.availableRoles = [...this.availableRoles, { ...role }];
  }

  onCancel() {
    this.userRoles = [...this.originalUserRoles];
  }

  onSubmit() {
    if (!this.userRolesParams.userId) return;
    if (!this.userRoles.length) {
      this.commonService.toast$.next({
        type: 'warn',
        message: 'Thành viên phải có ít nhất 1 vai trò',
      });
      return;
    }
    this.roleService
      .assignRole({
        userId: this.userRolesParams.userId,
        roleIds: this.userRoles.map((userRole) => userRole.id),
      })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.commonService.toast$.next({
            type: 'success',
            message: 'Gán vai trò thành công',
          });
          this.originalUserRoles = [...this.userRoles];
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
