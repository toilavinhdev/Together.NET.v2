import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  SvgDefinitionsComponent,
  ToastComponent,
} from '@/shared/components/elements';
import { Button } from 'primeng/button';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SvgDefinitionsComponent, ToastComponent, Button],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  constructor(private primengConfig: PrimeNGConfig) {}

  ngOnInit() {
    this.configPrimeNG();
  }

  private configPrimeNG() {
    this.primengConfig.ripple = true;
    this.primengConfig.zIndex = {
      modal: 1100, // dialog, sidebar
      overlay: 1000, // dropdown, overl aypanel
      menu: 1000, // overlay menus
      tooltip: 1100, // tooltip
    };
  }
}
