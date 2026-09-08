import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../app';
import { TaskDetails } from '../task-details/task-details';

@Component({
  selector: 'random-task-page',
  imports: [TaskDetails],
  templateUrl: './random-task-page.html',
  styleUrl: './random-task-page.scss',
})
export class RandomTaskPage implements OnInit {
  private http = inject(HttpClient);

  task = signal<Task | null>(null);

  // Get a random task from the API.
  ngOnInit(): void {
    this.http.get<Task>('/api/Tasks/random')
      .subscribe(data => this.task.set(data));
  }
}
