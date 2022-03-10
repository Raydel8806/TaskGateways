import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterGatewaysComponent } from './master-gateways.component';

describe('MasterGatewaysComponent', () => {
  let component: MasterGatewaysComponent;
  let fixture: ComponentFixture<MasterGatewaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MasterGatewaysComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterGatewaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
