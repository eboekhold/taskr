import { Component, input } from '@angular/core';
import { Task } from '../app';


@Component({
  selector: 'tr[task-list-item]',
  imports: [],
  templateUrl: './task-list-item.html',
  styleUrl: './task-list-item.scss',
})
export class TaskListItem {
  task = input<Task>();
}
