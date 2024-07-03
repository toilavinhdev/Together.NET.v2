import { Component, isDevMode } from '@angular/core';
import { RouterLink } from '@angular/router';
import { environment } from '@/environments/environment';

@Component({
  selector: 'together-main',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './main.component.html',
  styles: ``,
})
export class MainComponent {
  protected readonly isDevMode = isDevMode;
  protected readonly environment = environment;
}
