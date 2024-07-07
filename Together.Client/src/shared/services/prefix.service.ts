import { Injectable } from '@angular/core';
import { BaseService } from '@/core/abstractions';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { IPrefixViewModel } from '@/shared/entities/prefix.entities';
import { IBaseResponse } from '@/core/models';

@Injectable({
  providedIn: 'root',
})
export class PrefixService extends BaseService {
  prefixes$ = new BehaviorSubject<IPrefixViewModel[]>([]);

  constructor() {
    super();
    this.setEndpoint('/prefix');
  }

  listPrefix(): Observable<IPrefixViewModel[]> {
    const url = this.createUrl('list');
    return this.client
      .get<IBaseResponse<IPrefixViewModel[]>>(url)
      .pipe(map((response) => response.data));
  }
}
