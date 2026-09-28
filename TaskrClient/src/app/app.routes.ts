import { Routes } from '@angular/router';
import { RandomTaskPage } from './random-task-page/random-task-page';
import { TasksPage } from './tasks-page/tasks-page';
import { TaskPage } from './task-page/task-page';

export const routes: Routes = [
  {
    path: '',
    component: RandomTaskPage,
  },
  {
    path: 'tasks',
    component: TasksPage,
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
