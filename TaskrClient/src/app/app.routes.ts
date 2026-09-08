import { Routes } from '@angular/router';
import { RandomTaskPage } from './random-task-page/random-task-page';
import { Tasks } from './tasks/tasks';

export const routes: Routes = [
  {
    path: '',
    component: RandomTaskPage,
  },
  {
    path: 'tasks',
    component: Tasks,
  }
];
