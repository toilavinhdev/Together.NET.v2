import { Directive, inject, Input, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { CommonService } from '@/shared/services';
import { getErrorMessage } from '@/shared/utilities';

@Directive()
export class BaseComponent implements OnDestroy {
  @Input()
  componentClass = '';

  protected destroy$ = new Subject<void>();

  protected commonService = inject(CommonService);

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    this.destroy$.unsubscribe();
  }

  protected showToastError(err: any) {
    this.commonService.toast$.next({
      type: err,
      message: getErrorMessage(err),
    });
  }
}
