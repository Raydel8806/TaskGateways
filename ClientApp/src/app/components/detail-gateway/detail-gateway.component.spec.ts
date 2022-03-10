import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailGatewayComponent } from './detail-gateway.component';

describe('DetailGatewayComponent', () => {
  let component: DetailGatewayComponent;
  let fixture: ComponentFixture<DetailGatewayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetailGatewayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailGatewayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
