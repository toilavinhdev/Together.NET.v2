import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NavbarComponent } from '@/pages/main/_layout/components';
import { BaseComponent } from '@/core/abstractions';
import { UserService } from '@/shared/services';
import { takeUntil, tap } from 'rxjs';
import {getErrorMessage} from "@/shared/utilities";

@Component({
  selector: 'together-main',
  standalone: true,
  imports: [RouterLink, NavbarComponent, RouterOutlet],
  templateUrl: './main.component.html',
})
export class MainComponent extends BaseComponent implements OnInit {
  constructor(private userService: UserService) {
    super();
  }

  ngOnInit() {
    this.userService
      .getMe()
      .pipe(
        takeUntil(this.destroy$),
        tap(() => {
          this.commonService.spinning$.next(true);
        }),
      )
      .subscribe({
        next: (data) => {
          this.userService.me$.next(data);
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err)
          })
        },
        complete: () => {
          this.commonService.spinning$.next(false);
        },
      });
  }
}
