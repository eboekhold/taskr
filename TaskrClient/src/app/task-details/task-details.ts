import { Component, inject, input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../app';

@Component({
  selector: 'task-details',
  imports: [],
  templateUrl: './task-details.html',
  styleUrl: './task-details.scss',
})
export class TaskDetails {
  private http = inject(HttpClient);

  task = input<Task | null>(null);

  onCompletedChanged(event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;

    // Send a PUT request to the API and overwrite the isComplete property.
    this.http.put(`/api/Tasks/${this.task()?.id}`, { ...this.task(), isComplete: isChecked }).subscribe(
      // TODO: Do something interesting here, like handling errors and showing a toast message.
    );
  }
}
