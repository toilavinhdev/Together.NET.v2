import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from '@/pages/management/_layout/_components/menu/menu.component';
import { HeaderComponent } from '@/pages/management/_layout/_components/header/header.component';

@Component({
  selector: 'together-management',
  standalone: true,
  imports: [RouterOutlet, MenuComponent, HeaderComponent],
  templateUrl: './management.component.html',
})
export class ManagementComponent {}
