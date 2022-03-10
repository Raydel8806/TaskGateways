import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core'; 
import { Subscription } from 'rxjs/internal/Subscription';
import { Gateway } from '../../domain/gateway'; 
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { GatewaysService } from '../../service/gateways.service';
import { PeriphericalDevice } from '../../domain/peripherical-device'; 
@Component({
  selector: 'app-detail-gateway',
  templateUrl: './detail-gateway.component.html',
  styleUrls: ['./detail-gateway.component.css']
})
export class DetailGatewayComponent implements OnInit, OnDestroy { 
  public idGateway: number = 0;
  public gateway!: Gateway; 
  public returnedGateway!: any;
  public table = "All information of Gateway";
  public page: string = " Details of Gateway ";
  public errorDeletePeriphericalDevice: string = '';
  public idDeletePeriphericalDevice: number = -1;
  public deletedPeriphericalDevice!: PeriphericalDevice;
  public isAdmin: boolean = false; 

  constructor(public sGatewaysService: GatewaysService, public activatedRoute: ActivatedRoute)
  { 
  }

  ngOnDestroy(): void { 
  }

  ngOnInit(): void {
    this.idGateway = this.activatedRoute.snapshot.params['idGateway'];
    this.GetGateway();
  }
   
  GetGateway(): void {
    this.sGatewaysService.GetGateway(this.idGateway).subscribe({
      next: (res) => { this.gateway = res; },
      error: (e) => { this.errorDeletePeriphericalDevice = e; console.error(this.errorDeletePeriphericalDevice); }    });
  }

  public DeletePeriphericalDevice(idDeletePeriphericalDevice: number): void {
    this.idDeletePeriphericalDevice = idDeletePeriphericalDevice;
    this.sGatewaysService.DeletePeriphericalDevice(this.idGateway, this.idDeletePeriphericalDevice).subscribe({
      next: (res) => { this.deletedPeriphericalDevice = res; this.GetGateway(); },
      error: (e) => { this.errorDeletePeriphericalDevice = e; console.error(this.errorDeletePeriphericalDevice); }
    });
  }

  public DeleteAllPeriphericalDevice(): void {
    this.gateway.lsPeripheralDevices = new Array(0);
    this.sGatewaysService.UpdateGateway(this.gateway).subscribe({
      next: (res) => { this.gateway = res; },
      error: (e) => { this.errorDeletePeriphericalDevice = e; console.error(this.errorDeletePeriphericalDevice); }
    });
  }

   
}
