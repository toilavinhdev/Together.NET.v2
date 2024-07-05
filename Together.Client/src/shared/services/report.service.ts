import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { map, Observable } from 'rxjs';
import { IStatisticResponse } from '@/shared/entities/report.entities';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class ReportService extends BaseService {
  constructor() {
    super();
    this.setEndpoint('/report');
  }

  statistic(): Observable<IStatisticResponse> {
    const url = this.createUrl('/statistic');
    return this.client
      .get<IBaseResponse<IStatisticResponse>>(url)
      .pipe(map((response) => response.data));
  }
}
