import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { ReportService } from '@/shared/services';
import { takeUntil } from 'rxjs';
import {
  ContainerComponent,
  PrefixComponent,
} from '@/shared/components/elements';
import { MeterGroupModule, MeterItem } from 'primeng/metergroup';
import { NgForOf } from '@angular/common';
import { ProgressBarModule } from 'primeng/progressbar';
import { IPrefixReportResponse } from '@/shared/entities/report.entities';

@Component({
  selector: 'together-m-prefix-statistics',
  standalone: true,
  imports: [
    ContainerComponent,
    PrefixComponent,
    MeterGroupModule,
    NgForOf,
    ProgressBarModule,
  ],
  templateUrl: './m-prefix-statistics.component.html',
})
export class MPrefixStatisticsComponent
  extends BaseComponent
  implements OnInit
{
  loading = false;

  statistics: IPrefixReportResponse[] = [];

  constructor(private reportService: ReportService) {
    super();
  }

  ngOnInit() {
    this.loadReport();
  }

  private loadReport() {
    this.reportService
      .getPrefixReport()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.statistics = data;
        },
      });
  }
}
