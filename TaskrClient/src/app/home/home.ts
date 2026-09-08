import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../app';
import { TaskDetails } from '../task-details/task-details';

@Component({
  selector: 'app-home',
  imports: [TaskDetails],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  private http = inject(HttpClient);
  task = signal<Task | null>(null);

  ngOnInit(): void {
    this.http.get<Task>('/api/Tasks/random')
      .subscribe(data => this.task.set(data));
  }
}
