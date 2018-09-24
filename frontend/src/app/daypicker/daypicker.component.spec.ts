import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DaypickerComponent } from './daypicker.component';

describe('DaypickerComponent', () => {
  let component: DaypickerComponent;
  let fixture: ComponentFixture<DaypickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DaypickerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DaypickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
