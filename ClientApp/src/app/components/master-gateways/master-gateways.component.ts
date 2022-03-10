import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Gateway } from '../../domain/gateway';
import { Subscription } from 'rxjs';
import { GatewaysService } from '../../service/gateways.service';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-master-gateways',
  templateUrl: './master-gateways.component.html',
  styleUrls: ['./master-gateways.component.css']
})
export class MasterGatewaysComponent implements OnInit, OnDestroy {
  public gateways: Gateway[] = [];
  public subscription: Subscription = new Subscription();
  public sGatewaysService: GatewaysService;
  public router: Router;
  public table: string = "List of all registered gateways.";
  public page: string = " Gateways ";
  public deletedGateway: boolean = false;
  public errorDeletedGateway: any = '';
  public porcentOfCapacity: number = 0;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, gatewaysService: GatewaysService,router: Router) { 
    this.sGatewaysService = gatewaysService;
    this.router = router;
  }

  ngOnDestroy(): void {
       
  }

  ngOnInit(): void {
    this.GetAllGateways();
  }

  GetAllGateways(): void {
    this.subscription = this.sGatewaysService.GetAllGateway().subscribe(eGateways => { this.gateways = eGateways; }, error => console.error(error));
  }

  public deleteGateway(idGateway: number): void { 
    this.sGatewaysService.DeleteGateway(idGateway).subscribe({
      next: (res) => {
        this.deletedGateway = res;
        this.GetAllGateways()
        this.router.navigate(['/master - gateways']);

      },
      error: (e) => { this.errorDeletedGateway = e; console.error(this.errorDeletedGateway);}
    });
  }

  getCapacityPercent(max: number, val: number):void
  {
    this.porcentOfCapacity = (val / max) * 100;
  }

  //!!!future level Up
  public goEdit(idGateway: number): void
  {
    const route = 'add-gateway/' + true.valueOf() + '/' + idGateway.toString();
    console.log(route);
    this.router.navigate([route]);
  }
}


