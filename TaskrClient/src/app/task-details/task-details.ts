import { Component, input } from '@angular/core';
import { Task } from '../app';

@Component({
  selector: 'task-details',
  imports: [],
  templateUrl: './task-details.html',
  styleUrl: './task-details.scss',
})
export class TaskDetails {
  task = input<Task | null>(null);
}
