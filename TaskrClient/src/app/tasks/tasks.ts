import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Task {
  id: number;
  name: string;
  isComplete: boolean;
}

@Component({
  selector: 'app-tasks',
  imports: [],
  templateUrl: './tasks.html',
  styleUrl: './tasks.scss',
})
export class Tasks implements OnInit {
  private http = inject(HttpClient);
  tasks = signal<Task[]>([]);

  ngOnInit(): void {
    this.http.get<Task[]>('/api/Tasks')
      .subscribe(data => this.tasks.set(data));
  }
}
