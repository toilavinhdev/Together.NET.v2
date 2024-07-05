import { Component, Input } from '@angular/core';

@Component({
  selector: 'together-prefix',
  standalone: true,
  imports: [],
  templateUrl: './prefix.component.html',
})
export class PrefixComponent {
  @Input()
  name = '';

  @Input()
  foreground = '#000';

  @Input()
  background = '#FFF';
}
