import { Routes } from '@angular/router';

const ListPage = () => import('./list.page').then(m => m.ListPage);

export const ListRoutes: Routes = [
  { 
    path: '', 
    pathMatch: 'full', 
    loadComponent: ListPage 
  }
];