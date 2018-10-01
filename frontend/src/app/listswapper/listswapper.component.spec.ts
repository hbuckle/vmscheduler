import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListswapperComponent } from './listswapper.component';

describe('ListswapperComponent', () => {
  let component: ListswapperComponent;
  let fixture: ComponentFixture<ListswapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListswapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListswapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
