import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NavbarComponent } from '@/pages/main/_layout/components';

@Component({
  selector: 'together-main',
  standalone: true,
  imports: [RouterLink, NavbarComponent, RouterOutlet],
  templateUrl: './main.component.html',
})
export class MainComponent {}
