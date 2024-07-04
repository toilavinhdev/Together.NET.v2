import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService, CommonService } from '@/shared/services';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const commonService = inject(CommonService);

  if (!authService.getAT()) {
    commonService.navigateToLogin();
    return false;
  }

  return true;
};
