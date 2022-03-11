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

  public pageTitle: string = "";
  public pageTitleAddMode: string = "Add new Gateway";
  public pageTitleEditMode: string = "Editing Gateway: ";
  public idGateway: any='';
  public errorResult: any = '';
  public result: any = ''
  public submittedEdited: boolean = false;
  public isEditedMode: Byte = 0;

  //
  constructor(private gatewaysService: GatewaysService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.isEditedMode = this.route.snapshot.params['isEditedMode'];
    this.idGateway = this.route.snapshot.params['idGateway'];
    this.pageTitle = this.pageTitleAddMode;

    if (this.isEditedMode==1)
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
              this.pageTitle = this.pageTitleEditMode + this.gateway.name;
            },
            error: (e) =>
            {
              this.errorResult = e;
              console.error(this.errorResult.error);
            } 
      });
    }
  }

  submitGateway(): void {
    if (this.isValidData())
    {
      const newGateway = {
        id: this.gateway.id,
        serialNumber: this.gateway.serialNumber,
        name: this.gateway.name,
        ipAddress: this.gateway.ipAddress,
        lsPeripheralDevices: this.gateway.lsPeripheralDevices,
        maxClientNumber: this.gateway.maxClientNumber
      };

      if (this.isEditedMode==1)
      {
        this.gatewaysService.UpdateGateway(newGateway)
          .subscribe({
            next: (result) => {
              this.submittedEdited = true;
            },
            error: (e) => {
              this.errorResult = e; 

              if (e.error.errors != null) {
                if (e.error.errors.Name != null)
                { 
                  alert(e.error.errors.Name[0]); 
                }
                if (e.error.errors.SerialNumber!= null) {
                  alert(e.error.errors.SerialNumber[0]);
                }
              }
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
            error: (e) => {
              this.errorResult = e;
              console.error(this.errorResult);
              
            }
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
    this.isEditedMode = 0;
    this.pageTitle = this.pageTitleAddMode;
    this.gateway = {
      id: 0,
      serialNumber: "",
      name: "",
      ipAddress: "",
      lsPeripheralDevices: Array<PeriphericalDevice>(),
      maxClientNumber: 10
    };
  }

  isValidData(): boolean
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
