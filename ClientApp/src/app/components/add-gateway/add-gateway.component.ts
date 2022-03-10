import { Component, OnInit } from '@angular/core';
import { Byte } from '@angular/compiler/src/util';
import { ActivatedRoute } from '@angular/router'; 
import { Gateway } from '../../domain/gateway';
import { PeriphericalDevice } from '../../domain/peripherical-device';
import { GatewaysService } from '../../service/gateways.service';
  
@Component({
  selector: 'app-add-gateway',
  templateUrl: './add-gateway.component.html',
  styleUrls: ['./add-gateway.component.css']
})

export class AddGatewayComponent implements OnInit {
  gateway: Gateway = {
    id: 0,
    serialNumber : "",
    name : "",
    ipAddress: "",
    lsPeripheralDevices : Array <PeriphericalDevice>(),
    maxClientNumber: 10
  };

  public page: string = "";
  public pageAddMode: string = "Add new Gateway";
  public pageEditMode: string = "Editing Gateway: ";  
  public idGateway: any='';
  public errorResult: any = '';
  public result: any = ''
  public submittedEdited: boolean = false;
  public isEditedMode: any = false;

  //
  constructor(private gatewaysService: GatewaysService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.isEditedMode = this.route.snapshot.params['isEditedMode']; 
    this.idGateway = this.route.snapshot.params['idGateway'];
    this.page = this.pageAddMode;

    if (this.isEditedMode)
    { 
      this.gatewaysService
        .GetGateway(this
        .idGateway)
        .subscribe(
          {
            next: (result) =>
            {
              this.gateway = result; 
              //guardo la respuesta para agregar nuevos dispositivos a este gateway
              this.page = this.pageEditMode + this.idGateway;
            },
            error: (e) =>
            {
              this.errorResult = e;
              console.error(this.errorResult);
            }
/*** Upgrade late to show one for one case type!!!!
        error: (e) => {         
          if (e.status == 400){
            if (e.ModelError[0]) {
              this.error = e.ModelError[0];
            }
            else {
              this.error = e.error.title;
            }
          }
          ***/
      });
    }
  }

  submitGateway(): void {
    if (this.validData())
    {
      const newGateway = {
        id: this.gateway.id,
        serialNumber: this.gateway.serialNumber,
        name: this.gateway.name,
        ipAddress: this.gateway.ipAddress,
        lsPeripheralDevices: this.gateway.lsPeripheralDevices,
        maxClientNumber: this.gateway.maxClientNumber
      };

      if (this.isEditedMode == "true")
      {
        this.gatewaysService.UpdateGateway(newGateway)
          .subscribe({
            next: (result) => {
              this.result = result;
              this.submittedEdited = true;
            },
            error: (e) => {
              this.errorResult = e; console.error(this.errorResult);
              alert(e.status);
            }
          }
        );
      }
      else {
        this.gatewaysService.AddGateway(newGateway).subscribe(
          {
            next: (result) => {
              this.result = result;
              this.submittedEdited = true;
              this.gateway.id = (this.result as Gateway).id;

            },
            error: (e) => { this.errorResult = e; console.error(this.errorResult); }
          }
        ); 
      } 
    }
    else {
      alert(this.errorResult);
    }
  }
    
  resetGatewayData(): void {
    this.submittedEdited = false;
    this.isEditedMode = false;
    this.page = this.pageAddMode;
    this.gateway = {
      id: 0,
      serialNumber: "",
      name: "",
      ipAddress: "",
      lsPeripheralDevices: Array<PeriphericalDevice>(),
      maxClientNumber: 10
    };
  }

  validData(): boolean
  {
    if (this.gateway.serialNumber == '')
    {
      this.errorResult = "SerialNumber is Required.!!!";
    }
    else
    {
      if (this.gateway.name == '') {
        this.errorResult = "Name is Required.!!!";
      }
      else
      {
        if (this.gateway.ipAddress == '')
        {
          this.errorResult = "IpAddress is Required.!!!";
        }
        else
        {
          if (this.isValidIpAddress() == false)
          {
            this.errorResult = "IpAddress valid is Required.!!!";
          }
          else {
            this.errorResult = '';
            return true;
          }
        }
      }
    }
    return false;
  }

  isValidIpAddress(): boolean {
    let verdad = this.gateway.ipAddress.split('.');
  if (verdad.length != 4)
      return false;
    for (const i in verdad) {
    if (!/^\d+$/g.test(verdad[i]) || +verdad[i] > 255|| +verdad[i] < 0 || /^[0][0-9]{1,2}/.test(verdad[i]))
      return false;
  }
  return true
}
}
