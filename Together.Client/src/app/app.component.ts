import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  SvgDefinitionsComponent,
  ToastComponent,
} from '@/shared/components/elements';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SvgDefinitionsComponent, ToastComponent],
  templateUrl: './app.component.html',
})
export class AppComponent {}
