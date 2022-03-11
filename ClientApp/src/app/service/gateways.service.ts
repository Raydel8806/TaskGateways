import { Inject, Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Gateway } from '../domain/gateway';
import { ActivatedRoute } from '@angular/router';
import { PeriphericalDevice } from '../domain/peripherical-device';

@Injectable({
  providedIn: 'root'
})
  /*** * GatewaysService has methods for sending HTTP requests to the Apis. * ***/
  /*** * for reasons of time I will implement in a simple way.              * ***/
export class GatewaysService {

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json;'
    })
  };
  public baseUrl: string;
  public httpClient: HttpClient;

  constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute) {
    //this.url = './assets/miGateways.json'; 
    this.baseUrl = baseUrl;
    this.httpClient = httpClient;
    //this.rootUrl = 'https://my.api.mockaroo.com/gateways.json?key=824b3550';
  }

  //OK
  public GetAllGateway(): Observable<Gateway[]> {
    return this.httpClient.get<Gateway[]>(this.baseUrl + 'gateways');
  }
  //OK
  GetGateway(id: number): Observable<any> {
    return this.httpClient.get(this.baseUrl + "gateways/" + id.toString());
  }
  //OK
  AddGateway(gateway: any): Observable<any> {
    return this.httpClient.put(this.baseUrl + "gateways/", gateway, this.httpOptions);
  }
  //OK
  AddPeriphericalDevice(id: any, data: any): Observable<any> {
    return this.httpClient.post(this.baseUrl + "gateways/" + id, data, this.httpOptions);
  }
  //OK
  DeleteGateway(id: any): Observable<any> {
    return this.httpClient.delete(this.baseUrl +"gateways/" + id);
  }
  //OK
  DeletePeriphericalDevice(idGateway: any, idPDevice: any): Observable<any> {
    return this.httpClient.delete(this.baseUrl +"gateways?idGateway=" + idGateway + "&idPDevice=" + idPDevice);
  }
  //OK
  UpdateGateway(gateway: any): Observable<any> {
    return this.httpClient.patch(this.baseUrl + "gateways/", gateway);
  }
}
