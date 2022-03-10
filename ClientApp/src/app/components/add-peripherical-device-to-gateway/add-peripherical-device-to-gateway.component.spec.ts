import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPeriphericalDeviceToGatewayComponent } from './add-peripherical-device-to-gateway.component';

describe('AddPeriphericalDeviceToGatewayComponent', () => {
  let component: AddPeriphericalDeviceToGatewayComponent;
  let fixture: ComponentFixture<AddPeriphericalDeviceToGatewayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddPeriphericalDeviceToGatewayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddPeriphericalDeviceToGatewayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
