import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('@/pages/main/main.routes').then((r) => r.routes),
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('@/pages/auth/auth.routes').then((r) => r.routes),
  },
  {
    path: 'management',
    loadChildren: () =>
      import('@/pages/management/management.routes').then((r) => r.routes),
  },
];
