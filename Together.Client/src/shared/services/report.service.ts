import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { map, Observable } from 'rxjs';
import { IStatisticsResponse } from '@/shared/entities/report.entities';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class ReportService extends BaseService {
  constructor() {
    super();
    this.setEndpoint('/report');
  }

  statistics(metrics?: string[]): Observable<IStatisticsResponse> {
    const url = this.createUrl('/statistics');
    return this.client
      .post<IBaseResponse<IStatisticsResponse>>(url, { metrics })
      .pipe(map((response) => response.data));
  }
}
