import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Task } from '../app';

@Component({
  selector: 'tr[task-list-item]',
  imports: [RouterLink],
  templateUrl: './task-list-item.html',
  styleUrl: './task-list-item.scss',
})
export class TaskListItem {
  task = input<Task>();
}
