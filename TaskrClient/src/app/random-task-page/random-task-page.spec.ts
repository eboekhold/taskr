import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RandomTaskPage } from './random-task-page';

describe('RandomTaskPage', () => {
  let component: RandomTaskPage;
  let fixture: ComponentFixture<RandomTaskPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RandomTaskPage],
    }).compileComponents();

    fixture = TestBed.createComponent(RandomTaskPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
