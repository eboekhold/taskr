import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

import { mockTask } from '../../test/task.fixtures';

import { TaskPage } from './task-page';

describe('TaskPage', () => {
  let component: TaskPage;
  let fixture: ComponentFixture<TaskPage>;
  let httpClient: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskPage],
      providers: [provideHttpClientTesting(), provideRouter([{ path: 'tasks/:id', component: TaskPage }])],
    }).compileComponents();

    httpClient = TestBed.inject(HttpTestingController);

    fixture = TestBed.createComponent(TaskPage);
    component = fixture.componentInstance;
    component.taskId.set(mockTask.id.toString());
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should send exactly one request to the API', () => {
    const req = httpClient.expectOne(`/api/Tasks/${mockTask.id}`);
    req.flush(mockTask);
    TestBed.inject(HttpTestingController).verify();
  });
});
