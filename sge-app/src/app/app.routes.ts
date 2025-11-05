import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/employees/list',
    pathMatch: 'full'
  },
  {
    path: 'employees/list',
    loadChildren: () => import('./features/employees/pages/list/list.routes').then(m => m.ListRoutes)
  },
  {
    path: 'employees/form',
    loadChildren: () => import('./features/employees/pages/form/form.routes').then(m => m.FormRoutes)
  },
  {
    path: 'employees/form/:id',
    loadChildren: () => import('./features/employees/pages/form/form.routes').then(m => m.FormRoutes)
  }
];