import { HttpErrorResponse } from '@angular/common/http';
import { IBaseResponse } from '@/core/models';

export function getErrorMessage(error: Error): string {
  if (error instanceof HttpErrorResponse) {
    const response = error.error as IBaseResponse;
    return response?.errors?.[0].message ?? error.message;
  }
  return error.message;
}

export function isGUID(value: string) {
  const pattern =
    /^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}$/;
  return pattern.test(value);
}
