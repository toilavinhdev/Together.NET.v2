import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { NgTemplateOutlet } from '@angular/common';
import { ReportService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import { getErrorMessage } from '@/shared/utilities';
import { TranslateModule } from '@ngx-translate/core';
import { ShortenNumberPipe } from '@/shared/pipes';

@Component({
  selector: 'together-dashboard',
  standalone: true,
  imports: [NgTemplateOutlet, TranslateModule, ShortenNumberPipe],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent extends BaseComponent implements OnInit {
  private metrics = ['totalUser', 'totalPost', 'totalReply'];

  statistics: any;

  constructor(private reportService: ReportService) {
    super();
  }

  ngOnInit() {
    this.commonService.title$.next('Thống kê diễn đàn');
    this.loadStatistic();
  }

  private loadStatistic() {
    this.reportService
      .statistics(this.metrics)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.statistics = data;
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
