import { PeriphericalDevice } from "./peripherical-device"

export interface Gateway {
  id: number;
  serialNumber: string;
  name: string;
  ipAddress: string;
  lsPeripheralDevices: Array<PeriphericalDevice>;
  maxClientNumber: number;
}             
