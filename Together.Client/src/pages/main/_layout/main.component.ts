import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'together-main',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './main.component.html',
})
export class MainComponent {}
