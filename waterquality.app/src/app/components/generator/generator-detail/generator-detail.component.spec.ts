import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratorDetailComponent } from './generator-detail.component';

describe('GeneratorDetailComponent', () => {
  let component: GeneratorDetailComponent;
  let fixture: ComponentFixture<GeneratorDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratorDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratorDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
