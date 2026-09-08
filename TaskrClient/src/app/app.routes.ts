import { Routes } from '@angular/router';
import { RandomTaskPage } from './random-task-page/random-task-page';
import { Tasks } from './tasks/tasks';
import { TaskPage } from './task-page/task-page';

export const routes: Routes = [
  {
    path: '',
    component: RandomTaskPage,
  },
  {
    path: 'tasks',
    component: Tasks,
  },
  {
    path: 'tasks/:id',
    component: TaskPage,
  },
  {
    path: 'tasks/random',
    component: RandomTaskPage,
  }
];
