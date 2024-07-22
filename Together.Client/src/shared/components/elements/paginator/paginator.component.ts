import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaginatorModule } from 'primeng/paginator';
import { BaseComponent } from '@/core/abstractions';

export interface IPaginatorChange {
  pageIndex: number;
}

@Component({
  selector: 'together-paginator',
  standalone: true,
  imports: [PaginatorModule],
  templateUrl: './paginator.component.html',
})
export class PaginatorComponent extends BaseComponent {
  @Input()
  pageIndex = 1;

  @Input()
  pageSize = 5;

  @Input()
  totalRecord = 50;

  @Input()
  showCurrentPageReport = false;

  @Output()
  paginationChange = new EventEmitter<IPaginatorChange>();

  onPageChange(event: any) {
    this.paginationChange.emit({
      pageIndex: event.page + 1,
    });
  }
}
