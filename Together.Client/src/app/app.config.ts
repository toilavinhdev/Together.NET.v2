import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {
  HttpInterceptorFn,
  provideHttpClient,
  withInterceptors,
} from '@angular/common/http';
import { tokenInterceptor } from '@/core/interceptors';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

const interceptors: HttpInterceptorFn[] = [tokenInterceptor];

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors(interceptors)),
    provideAnimationsAsync(),
  ],
};
