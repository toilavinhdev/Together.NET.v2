import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SvgIconComponent } from '@/shared/components/elements';
import { Button } from 'primeng/button';

@Component({
  selector: 'together-navbar',
  standalone: true,
  imports: [RouterLink, SvgIconComponent, Button],
  templateUrl: './navbar.component.html',
})
export class NavbarComponent {}
