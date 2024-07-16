import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  SpinnerComponent,
  SvgDefinitionsComponent,
  ToastComponent,
} from '@/shared/components/elements';
import { PrimeNGConfig } from 'primeng/api';
import {environment} from "@/environments/environment";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SvgDefinitionsComponent,
    ToastComponent,
    SpinnerComponent,
  ],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  constructor(private primengConfig: PrimeNGConfig) {}

  ngOnInit() {
    console.log('production', environment.production)
    this.configPrimeNG();
  }

  private configPrimeNG() {
    this.primengConfig.ripple = true;
    this.primengConfig.zIndex = {
      modal: 101, // dialog, sidebar
      overlay: 10, // dropdown, overlay panel
      menu: 10, // overlay menus
      tooltip: 10, // tooltip
    };
  }
}
