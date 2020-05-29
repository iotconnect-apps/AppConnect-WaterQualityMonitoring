import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TelemetryPopupComponent } from './telemetry-popup.component';

describe('TelemetryPopupComponent', () => {
  let component: TelemetryPopupComponent;
  let fixture: ComponentFixture<TelemetryPopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TelemetryPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TelemetryPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
