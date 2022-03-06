using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusalaGatewaysSysAdmin.Models;

namespace MusalaGatewaysSysAdmin.Models
{
    /*
    public class MockGatewayRepository 
    {
        private List<Gateway> _gatewayRepository; 

        public MockGatewayRepository()
        { 
            _gatewayRepository = new List<Gateway>(){
                new Gateway() {SerialNumber = "100010001", ID=1, IpAddress = "1.1.1.1", Name = "Gateway01",
                    LsPeripheralDevices = new List<PeripheralDevice>(){
                        new PeripheralDevice(){ ID = 100,UId = 88061522388, DtDeviceCreated = DateTime.Now, DeviceVendor="ASUS",Online=true},
                        new PeripheralDevice(){ ID = 103,UId = 52238888061, DtDeviceCreated = DateTime.Now, DeviceVendor="BIOSTAR",Online=false},
                        new PeripheralDevice(){ ID = 104,UId = 80615282388, DtDeviceCreated = DateTime.Now, DeviceVendor="DELL",Online=true},
                        new PeripheralDevice(){ ID = 107,UId = 06152288388, DtDeviceCreated = DateTime.Now, DeviceVendor="INTEL",Online=true}
                    }, MaxClientNumber = 10
                },
                new Gateway() {SerialNumber = "100010002", ID=2, IpAddress = "1.1.1.2", Name = "Gateway02",
                    LsPeripheralDevices = new List<PeripheralDevice>(){
                        new PeripheralDevice(){ ID = 120,UId = 88880615223, DtDeviceCreated = DateTime.Now, DeviceVendor="DELL",Online=true},
                        new PeripheralDevice(){ ID = 123,UId = 52888061238, DtDeviceCreated = DateTime.Now, DeviceVendor="INTEL",Online=false},
                        new PeripheralDevice(){ ID = 124,UId = 52828061806, DtDeviceCreated = DateTime.Now, DeviceVendor="DELL",Online=true},
                        new PeripheralDevice(){ ID = 127,UId = 06806288388, DtDeviceCreated = DateTime.Now, DeviceVendor="INTEL",Online=true}
                    }, MaxClientNumber = 10
                },
                new Gateway() {SerialNumber = "100010003", ID=1, IpAddress = "1.1.1.3", Name = "Gateway03",
                    LsPeripheralDevices = new List<PeripheralDevice>(){
                        new PeripheralDevice(){ ID = 100,UId = 88061522388, DtDeviceCreated = DateTime.Now, DeviceVendor="ASUS",Online=true},
                        new PeripheralDevice(){ ID = 103,UId = 52238888061, DtDeviceCreated = DateTime.Now, DeviceVendor="BIOSTAR",Online=false},
                        new PeripheralDevice(){ ID = 104,UId = 80615282388, DtDeviceCreated = DateTime.Now, DeviceVendor="DELL",Online=true},
                        new PeripheralDevice(){ ID = 107,UId = 06152288388, DtDeviceCreated = DateTime.Now, DeviceVendor="INTEL",Online=true}
                    }, MaxClientNumber = 10
                },
                new Gateway() {SerialNumber = "100010004", ID=2, IpAddress = "1.1.1.4", Name = "Gateway04",
                    LsPeripheralDevices = new List<PeripheralDevice>(){
                        new PeripheralDevice(){ ID = 120,UId = 88880615223, DtDeviceCreated = DateTime.Now, DeviceVendor="DELL",Online=true},
                        new PeripheralDevice(){ ID = 123,UId = 52888061238, DtDeviceCreated = DateTime.Now, DeviceVendor="INTEL",Online=false},
                        new PeripheralDevice(){ ID = 124,UId = 52828061806, DtDeviceCreated = DateTime.Now, DeviceVendor="DELL",Online=true},
                        new PeripheralDevice(){ ID = 127,UId = 06806288388, DtDeviceCreated = DateTime.Now, DeviceVendor="INTEL",Online=true}
                    }, MaxClientNumber = 10
                }

            };
        }

        ActionResult<Gateway> IGatewayRepository.AddDeviceToGateway(int gatewayId, PeripheralDevice peripheralDevice)
        {           
            Gateway? nGateway = _gatewayRepository.FirstOrDefault(e => e.ID == gatewayId);

            if (nGateway != null)
            {
                if (nGateway.LsPeripheralDevices == null)
                {
                    nGateway.LsPeripheralDevices = new List<PeripheralDevice>() { peripheralDevice };
                }
                else
                {
                    if (nGateway.LsPeripheralDevices.Count < nGateway.MaxClientNumber)
                    {
                        nGateway.LsPeripheralDevices.Add(peripheralDevice);
                    }
                    else
                    {
                        throw new Exception("The maximum number of devices already exists.");
                    }

                }
                return nGateway;
            }
            else
            {
                throw new Exception("Unregistred Gateway");
            }
        }

        Gateway IGatewayRepository.AddGateway(Gateway gateway)
        {
            _gatewayRepository.Add(gateway); 
            return gateway;
        }

        Gateway IGatewayRepository.DeleteDeviceFromGateway(int gatewayId, int peripheralDeviceId)
        {
            Gateway? nGateway = _gatewayRepository.FirstOrDefault(e => e.ID == gatewayId);

            if (nGateway != null)
            {
                if (nGateway.LsPeripheralDevices == null)
                {
                    throw new Exception("Unregistred Gateway");
                }
                else{
                    PeripheralDevice? ePeripheralDevice = nGateway.LsPeripheralDevices.FirstOrDefault(e => e.ID == peripheralDeviceId);

                    if (ePeripheralDevice != null)
                    {
                        nGateway.LsPeripheralDevices.Remove(ePeripheralDevice);
                    }
                    else{
                        throw new Exception("The maximum number of devices already exists.");
                    }

                }
                return nGateway;
            }
            else
            {
                throw new Exception("Unregistred Gateway");
            }
        }

        Gateway IGatewayRepository.DeleteGateway(int gatewayId)
        {
            var nGateway = this._gatewayRepository.First(e => e.ID == gatewayId);
 
            if (nGateway != null)
            {
                this._gatewayRepository.Remove(nGateway); 
                return nGateway;
            }
            else
            {
                throw new Exception("Unregistred Gateway");
            }
        }

        IEnumerable<Gateway> IGatewayRepository.GetAllGateways()
        {
            return _gatewayRepository;
        }

        Gateway IGatewayRepository.GetGateway(int gatewayId)
        {
            Gateway? nGateway = _gatewayRepository.FirstOrDefault(e => e.ID == gatewayId);
            if (nGateway != null)
            {
                return nGateway;
            }
            else
            {
                throw new Exception("Unregistred Gateway");
            }
        }

        Gateway? IGatewayRepository.GetGatewayBySerialNumber(string serialNumber)
        {
            Gateway? nGateway = _gatewayRepository.FirstOrDefault(e => e.SerialNumber == serialNumber);
            return nGateway;
        }
    }
  */}
