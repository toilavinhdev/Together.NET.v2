import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { IStatisticResponse } from '@/shared/entities/report.entities';
import { ReportService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { NgClass } from '@angular/common';
import { ShortenNumberPipe } from '@/shared/pipes';
import { getErrorMessage } from '@/shared/utilities';

@Component({
  selector: 'together-statistic',
  standalone: true,
  imports: [NgClass, ShortenNumberPipe],
  templateUrl: './statistic.component.html',
})
export class StatisticComponent extends BaseComponent implements OnInit {
  statistic!: IStatisticResponse;

  loading = false;

  constructor(private reportService: ReportService) {
    super();
  }

  ngOnInit() {
    this.reportService
      .statistic()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.statistic = data;
        },
        error: (err) => {
          this.commonService.toast$.next({
            type: 'error',
            message: getErrorMessage(err),
          });
        },
      });
  }
}
