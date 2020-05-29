import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratorAddComponent } from './generator-add.component';

describe('DeviceAddComponent', () => {
  let component: GeneratorAddComponent;
  let fixture: ComponentFixture<GeneratorAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratorAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratorAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
