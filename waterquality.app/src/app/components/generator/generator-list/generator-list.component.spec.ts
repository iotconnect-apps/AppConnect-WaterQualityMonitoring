import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratorListComponent } from './generator-list.component';

describe('GeneratorListComponent', () => {
  let component: GeneratorListComponent;
  let fixture: ComponentFixture<GeneratorListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratorListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
