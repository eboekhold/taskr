import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { mockTaskList } from '../../test/task.fixtures';

import { TasksPage } from './tasks-page';

describe('TasksPage', () => {
  let component: TasksPage;
  let fixture: ComponentFixture<TasksPage>;
  let httpClient: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TasksPage],
      providers: [provideHttpClientTesting()],
    }).compileComponents();

    httpClient = TestBed.inject(HttpTestingController);

    fixture = TestBed.createComponent(TasksPage);
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
