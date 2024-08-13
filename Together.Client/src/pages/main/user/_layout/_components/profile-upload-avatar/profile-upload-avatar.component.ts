import { Component, OnInit } from '@angular/core';
import { FileUploadModule } from 'primeng/fileupload';
import { BaseComponent } from '@/core/abstractions';
import { TooltipModule } from 'primeng/tooltip';
import { UserService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-profile-upload-avatar',
  standalone: true,
  imports: [FileUploadModule, TooltipModule],
  templateUrl: './profile-upload-avatar.component.html',
})
export class ProfileUploadAvatarComponent
  extends BaseComponent
  implements OnInit
{
  constructor(private userService: UserService) {
    super();
  }

  ngOnInit() {}

  onFileChange(event: any) {
    const file = event.target.files[0];
    console.log(1, file);
    this.commonService.toast$.next({
      type: 'info',
      message: 'Đang tải ảnh lên server...',
    });
    this.userService
      .uploadAvatar(file)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (url) => {
          this.updateAvatar(url);
          this.commonService.toast$.next({
            type: 'success',
            message: 'Cập nhật ảnh đại diện thành công',
          });
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }

  private updateAvatar(url: string) {
    if (!this.userService.me$.value) return;
    this.userService.me$.next({ ...this.userService.me$.value, avatar: url });
    if (this.userService.me$.value.id === this.userService.user$.value?.id) {
      this.userService.user$.next({
        ...this.userService.user$.value,
        avatar: url,
      });
    }
  }
}
