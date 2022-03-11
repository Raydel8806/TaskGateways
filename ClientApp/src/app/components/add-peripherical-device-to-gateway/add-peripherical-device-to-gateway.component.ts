import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { Gateway } from '../../domain/gateway';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { GatewaysService } from '../../service/gateways.service';
import { PeriphericalDevice } from '../../domain/peripherical-device';
import { DatePipe } from '@angular/common' 
 
@Component({
  selector: 'app-add-peripherical-device-to-gateway',
  templateUrl: './add-peripherical-device-to-gateway.component.html',
  styleUrls: ['./add-peripherical-device-to-gateway.component.css']
})
export class AddPeriphericalDeviceToGatewayComponent implements OnInit {

  periphericalDevice: PeriphericalDevice = {
    id: 0,
    uId: 0,
    deviceVendor: '',
    dtDeviceCreated: new Date(),
    online: 'false',
    gatewayID:0 
  };

  public table: string = "Add new Peripherical Device to Gateway: ";
  public page: string = "New Peripherical Device";
  public errorAddPeriphericalDevice: any = '';
  public result: any = ''
  public submittedPeriphericalDevice: boolean = false;
  public idGateway: any = ''; 
  public nameGateway: any ='';

  constructor(private gatewaysService: GatewaysService, private route: ActivatedRoute, public datepipe: DatePipe) { }

  ngOnInit(): void {
    this.idGateway = this.route.snapshot.params['idGateway'];
    this.nameGateway = this.route.snapshot.params['nameGateway'];
    this.table = this.table + this.nameGateway; 
  }

  submitPeriphericalDevice(): void {
     
    if (this.validData())
    {
      const newPeriphericalDevice = {
        id: 0,
        uId: this.periphericalDevice.uId,
        deviceVendor: this.periphericalDevice.deviceVendor,
        dtDeviceCreated: this.getDate('yyyy-MM-dd'),
        online: this.periphericalDevice.online === 'true' ? true : false,
        gatewayID: 0
      };

      this.gatewaysService
        .AddPeriphericalDevice(this
        .idGateway, newPeriphericalDevice)
        .subscribe({
            next: (result) => {
              console.info(result);
              this.submittedPeriphericalDevice = true; 
            },
            error: (e) =>{ 
              console.error(e);
              alert(e.error);
            }
          });
    }
    else
    {
      alert(this.errorAddPeriphericalDevice);
    }

  }

  validData(): boolean {
    if (this.periphericalDevice.uId <= 0) {
      this.errorAddPeriphericalDevice = "UId is Required.!!!";
      
    }
    else {
      if (this.periphericalDevice.deviceVendor == '') {
        this.errorAddPeriphericalDevice = "Name is Required.!!!";
      }
      else
      {
        return true;
      }
    }  

    return false;
  }

  getDate(format: string): string {
    const dtDeviceCreated = new Date();
    return this.datepipe.transform(dtDeviceCreated, format)!;
  }

  resetPeriphericalDeviceData(): void {
    this.submittedPeriphericalDevice = false;
    this.periphericalDevice = {
      id: 0,
      uId: 0,
      deviceVendor: '',
      dtDeviceCreated: new Date(),
      online: 'false',
      gatewayID: this.idGateway
    };
  }
}
