import { DatePipe } from "@angular/common";

/*
export class PeriphericalDevice {
  /* 
   * PeripheralDevice{
   *  id	integer($int32)
   *  uId*	integer($int64)
   *  deviceVendor*	string
   *  dtDeviceCreated*	string($date-time)
   *  online*	boolean
   *  gatewayID	integer($int32)
   *  }
   
  constructor(
    public id: number,
    public uId: number,
    public deviceVendor: string,
    public dtDeviceCreated: string,
    public online: boolean,
    public gatewayID: number,
  ) { }
}
*/
export interface PeriphericalDevice {
  id: number;
  uId: number;
  deviceVendor: string;
  dtDeviceCreated: Date;
  online: string;
  gatewayID: number;
}
