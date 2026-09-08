import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { mockTask } from '../../test/task.fixtures';

import { RandomTaskPage } from './random-task-page';

describe('RandomTaskPage', () => {
  let component: RandomTaskPage;
  let fixture: ComponentFixture<RandomTaskPage>;
  let httpClient: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RandomTaskPage],
      providers: [provideHttpClientTesting()],
    }).compileComponents();

    httpClient = TestBed.inject(HttpTestingController);

    fixture = TestBed.createComponent(RandomTaskPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should send exactly one request to the API', () => {
    const req = httpClient.expectOne('/api/Tasks/random');
    req.flush(mockTask);
    TestBed.inject(HttpTestingController).verify();
  })
});
