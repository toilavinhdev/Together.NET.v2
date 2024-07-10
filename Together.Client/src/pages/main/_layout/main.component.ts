import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import {
  BreadcrumbComponent,
  NavbarComponent,
} from '@/pages/main/_layout/components';
import { BaseComponent } from '@/core/abstractions';
import { UserService, WebSocketService } from '@/shared/services';
import { takeUntil, tap } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { IWebSocketMessage } from '@/core/models';

@Component({
  selector: 'together-main',
  standalone: true,
  imports: [RouterLink, NavbarComponent, RouterOutlet, BreadcrumbComponent],
  templateUrl: './main.component.html',
})
export class MainComponent extends BaseComponent implements OnInit {
  constructor(
    private userService: UserService,
    private webSocketService: WebSocketService,
  ) {
    super();
  }

  ngOnInit() {
    this.getMe();
    this.webSocket();
  }

  private getMe() {
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
            message: getErrorMessage(err),
          });
        },
        complete: () => {
          this.commonService.spinning$.next(false);
        },
      });
  }

  private webSocket() {
    this.webSocketService.client$.subscribe((message: IWebSocketMessage) => {
      console.log('ws ', message);
    });
  }
}
