import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Subscription } from 'rxjs';
import { Gateway } from '../domain/gateway';


@Component({
  selector: 'app-gateways',
  templateUrl: './gateways.component.html'
})
export class GatewaysComponent {
  public gateways: Gateway[] = [];
  public subGateways: Subscription;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.subGateways = new Subscription();
    http.get<Gateway[]>(baseUrl + 'gateways').subscribe(result => {
      this.gateways = result;
    }, error => console.error(error));
  }
}
