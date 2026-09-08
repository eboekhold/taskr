import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { mockTaskList } from '../../test/task.fixtures';

import { TaskList } from './task-list';

describe('TaskList', () => {
  let component: TaskList;
  let fixture: ComponentFixture<TaskList>;
  let httpClient: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskList],
      providers: [provideHttpClientTesting()],
    }).compileComponents();

    httpClient = TestBed.inject(HttpTestingController);

    fixture = TestBed.createComponent(TaskList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should send exactly one request to the API', () => {
    const req = httpClient.expectOne('/api/Tasks');
    req.flush(mockTaskList);
    TestBed.inject(HttpTestingController).verify();
  });
});
