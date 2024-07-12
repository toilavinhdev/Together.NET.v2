import { Injectable } from '@angular/core';
import { webSocket } from 'rxjs/webSocket';
import { interval } from 'rxjs';
import { IWebSocketMessage } from '@/core/models/websocket.models';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  client$ = webSocket<IWebSocketMessage>(this.createUrl());

  constructor() {
    this.keepConnect();
  }

  private createUrl() {
    return 'ws://localhost:5005/ws?id=512fefc3-0cb5-42c2-a7f8-1c171f65559e';
  }

  private keepConnect() {
    interval(60 * 1000).subscribe(() => {
      this.client$.next({
        target: 'Ping',
      });
    });
  }

  public disconnect() {
    this.client$.complete();
    this.client$.unsubscribe();
  }
}
