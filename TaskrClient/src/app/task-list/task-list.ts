import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TaskListItem } from '../task-list-item/task-list-item';
import { Task } from '../app';

@Component({
  selector: 'task-list',
  imports: [TaskListItem],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskList implements OnInit {
  private http = inject(HttpClient);
  tasks = signal<Task[]>([]);

  ngOnInit(): void {
    this.http.get<Task[]>('/api/Tasks')
      .subscribe(data => this.tasks.set(data));
  }
}
