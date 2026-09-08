import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { TaskPage } from '../task-page/task-page';
import { TaskListItem } from './task-list-item';

describe('TaskListItem', () => {
  let component: TaskListItem;
  let fixture: ComponentFixture<TaskListItem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskListItem],
      providers: [provideRouter([{ path: 'tasks/:id', component: TaskPage }])],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskListItem);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
