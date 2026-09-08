import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Task } from '../app';
import { TaskDetails } from '../task-details/task-details';

@Component({
  selector: 'app-task-page',
  imports: [TaskDetails],
  templateUrl: './task-page.html',
  styleUrl: './task-page.scss',
})
export class TaskPage implements OnInit {
  private http = inject(HttpClient);
  private route = inject(ActivatedRoute);

  taskId = signal<string | null>(null);
  task = signal<Task | null>(null);

  constructor() {
    this.route.paramMap.subscribe(params => {
      this.taskId.set(params.get('id'));
    });
  }

  ngOnInit(): void {
    this.http.get<Task>(`/api/Tasks/${this.taskId()}`)
      .subscribe(data => this.task.set(data));
  }
}