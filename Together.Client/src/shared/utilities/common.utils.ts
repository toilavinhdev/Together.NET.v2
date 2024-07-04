import { HttpErrorResponse } from '@angular/common/http';
import { IBaseResponse } from '@/core/models';

export function getErrorMessage(error: Error): string {
  if (error instanceof HttpErrorResponse) {
    const response = error.error as IBaseResponse;
    return response?.errors?.[0].message ?? error.message;
  }
  return error.message;
}
