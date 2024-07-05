import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService, CommonService } from '@/shared/services';
import { catchError, throwError } from 'rxjs';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const commonService = inject(CommonService);
  const at = authService.getAT();

  const requestWithAT = () => {
    if (!at) return req;
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${at}`,
      },
    });
  };

  return next(requestWithAT()).pipe(
    catchError((err) => {
      if (err instanceof HttpErrorResponse && err.status === 401) {
        commonService.toast$.next({
          type: 'error',
          message: 'Phiên đăng nhập hết hạn',
        });
      }

      return throwError(() => err);
    }),
  );
};
