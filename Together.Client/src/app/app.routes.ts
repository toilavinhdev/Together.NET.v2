import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('@/pages/main/_layout/main.routes').then((o) => o.routes),
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('@/pages/auth/_layout/auth.routes').then((o) => o.routes),
  },
  {
    path: 'management',
    loadChildren: () =>
      import('@/pages/management/_layout/management.routes').then(
        (o) => o.routes,
      ),
  },
];
