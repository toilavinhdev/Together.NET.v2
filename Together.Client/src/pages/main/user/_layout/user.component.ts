import { Component } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ProfileInfoComponent } from './_components/profile-info/profile-info.component';

@Component({
  selector: 'together-user',
  standalone: true,
  imports: [ProfileInfoComponent],
  templateUrl: './user.component.html',
})
export class UserComponent extends BaseComponent {}
