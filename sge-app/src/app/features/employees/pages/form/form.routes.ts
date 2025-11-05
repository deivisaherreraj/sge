import { Routes } from '@angular/router';

const FormPage = () => import('./form.page').then(m => m.FormPage);

export const FormRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: FormPage
  }
];