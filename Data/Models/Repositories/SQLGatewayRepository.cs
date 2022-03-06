using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusalaGatewaysSysAdmin.Models;
using System.Text.RegularExpressions;

namespace MusalaGatewaysSysAdmin.Models
{
    public class SQLGatewayRepository : IGatewayRepository
    { 
        private readonly IDbContextFactory<GatewaysSysAdminDBContext> _contextFactory;

        public SQLGatewayRepository(IDbContextFactory<GatewaysSysAdminDBContext> gatewaysSysAdminDBContextFactory)
        {
            this._contextFactory = gatewaysSysAdminDBContextFactory;
        }
        async Task<ActionResult<Gateway>> IGatewayRepository.AddGateway(Gateway gateway)
        {
            try
            {
                using var cSysAdminDBContext = _contextFactory.CreateDbContext();
                await cSysAdminDBContext.Gateway.AddAsync(gateway);
                await cSysAdminDBContext.SaveChangesAsync();

                var rGateway = await GetGatewayBySerialNumber(gateway.SerialNumber!);

                if (rGateway != null)
                {
                    rGateway = LoadPeripheralDevices(rGateway.Value!).Result;
                    return rGateway;
                }
                else
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }//ok
        /// <summary>
        /// Get All Gateways in Data Base.
        /// </summary>
        /// <returns>ActionResult<IEnumerable<Gateway>></returns>
         IEnumerable<Gateway> IGatewayRepository.GetAllGateways()
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();
            var eAllGateways = cSysAdminDBContext.Gateway.ToListAsync().Result;

            using var c1SysAdminDBContext = _contextFactory.CreateDbContext();
            var jAllPeripheralDevices =  c1SysAdminDBContext.PeripheralDevice.ToListAsync().Result;

            foreach (var iGateway in eAllGateways)
            {
                iGateway.LsPeripheralDevices = LoadPeripheralDevices(iGateway).Result.LsPeripheralDevices;        
            }

            return eAllGateways;
        }//ok

        async Task<ActionResult<Gateway>> IGatewayRepository.GetGateway(int gatewayId)
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();
            var rGateway = await cSysAdminDBContext.Gateway.FindAsync(gatewayId);

            if (rGateway != null)
            {
                rGateway = LoadPeripheralDevices(rGateway).Result;
                return rGateway;
            }
            else
            {
                return null!;
            }
        }//ok
         
        async Task<ActionResult<Gateway>> IGatewayRepository.DeleteGateway(int gatewayId)
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();
            var rGateway = cSysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.ID == gatewayId).Result;

            if (rGateway != null)
            {
                rGateway = LoadPeripheralDevices(rGateway).Result;

                using var c1SysAdminDBContext = _contextFactory.CreateDbContext();
                c1SysAdminDBContext.Gateway.Remove(rGateway);

                await c1SysAdminDBContext.SaveChangesAsync();

                foreach (var iPeriphericalDevice in rGateway.LsPeripheralDevices)
                {
                    using var c2SysAdminDBContext = _contextFactory.CreateDbContext();
                    c1SysAdminDBContext.PeripheralDevice.Remove(iPeriphericalDevice);
                    await c2SysAdminDBContext.SaveChangesAsync();
                }

                return rGateway;
            }
            return null!;
        }//OK

        ActionResult<Gateway>IGatewayRepository.UpdateGateway(Gateway gateway)//OK
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();
            cSysAdminDBContext.Entry(gateway).State = EntityState.Modified;
            cSysAdminDBContext.SaveChanges();

            /*            
            var rGateway = cSysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.ID == gateway.ID).Result;
            if (rGateway != null)
            {
                rGateway = LoadPeripheralDevices(rGateway).Result;
                rGateway.SerialNumber = gateway.SerialNumber;
                rGateway.IpAddress = gateway.IpAddress;
                rGateway.LsPeripheralDevices = gateway.LsPeripheralDevices;
                rGateway.MaxClientNumber = gateway.MaxClientNumber;
                cSysAdminDBContext.SaveChanges();
                rGateway = cSysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.ID == gateway.ID).Result;

                if (rGateway != null)
                {
                    return rGateway;
                } 
            }*/
            return gateway;             
        }

        async Task<ActionResult<Gateway>> IGatewayRepository.AddDeviceToGateway(int gatewayId, PeripheralDevice peripheralDevice)
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();

            Gateway? eGateway = await cSysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.ID == gatewayId);
            
            if (eGateway == null)
            {
                throw new Exception("Unregistred Gateway");
            }
            else
            {
                var lsPeripheralDevices =  LoadPeripheralDevices(eGateway).Result.LsPeripheralDevices;

                if (lsPeripheralDevices == null)
                {
                    eGateway.LsPeripheralDevices = new List<PeripheralDevice>()
                    {
                        peripheralDevice
                    };
                }
                else
                {
                    if (lsPeripheralDevices.Any(e => e.ID == peripheralDevice.ID))
                    {
                        throw new Exception("Device already exists in Gateway.");
                    } 
                    else
                    {
                        if (eGateway.LsPeripheralDevices.Count < eGateway.MaxClientNumber)
                        { 
                            eGateway.LsPeripheralDevices.Add(peripheralDevice);

                            cSysAdminDBContext.SaveChanges(); 

                            return eGateway;
                            
                        }
                        else
                        {
                            throw new Exception("This Gateway can not accept more clients.");

                        }

                    } 
                }

                cSysAdminDBContext.Attach(eGateway);
                await cSysAdminDBContext.SaveChangesAsync();

                if (await IsDeviceInGateway(gatewayId, peripheralDevice.ID))
                {
                    return eGateway;
                }
                else
                {
                    return null!;
                } 

            }
        }

        async Task<ActionResult<Gateway>> IGatewayRepository.DeleteDeviceFromGateway(int gatewayId, int peripheralDeviceId)
        {
            using var c1SysAdminDBContext = _contextFactory.CreateDbContext();
            var ePeripheralDevice = await c1SysAdminDBContext.PeripheralDevice.FirstOrDefaultAsync(e => e.ID == peripheralDeviceId);

            using var cSysAdminDBContext = _contextFactory.CreateDbContext();
            var eGateway = await cSysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.ID == gatewayId);
                       
            if (eGateway == null)
            {
                throw new Exception("Gateway does not exist.");
            }
            else
            {
                if (ePeripheralDevice == null)
                {
                    throw new Exception("Peripherical Device does not exist."); 
                }
                else
                {
                    if (eGateway.ID==ePeripheralDevice.GatewayID)
                    {
                        List<PeripheralDevice> lsPeripheralDevices = LoadPeripheralDevices(eGateway).Result.LsPeripheralDevices.ToList();
                       
                        if (lsPeripheralDevices.Count > 0)
                        {                             
                            if (lsPeripheralDevices.RemoveAll(eP=>eP.ID==peripheralDeviceId)>=1)
                            { 
                                cSysAdminDBContext.Entry(eGateway).DetectChanges();
                                eGateway.LsPeripheralDevices = lsPeripheralDevices;
                                cSysAdminDBContext.SaveChanges();
                                return eGateway;
                            }

                        }
                                              
                    }
                    else
                    { 
                        throw new Exception("Device does not exists in this Gateway.");
                    }
                } 

                if (await IsDeviceInGateway(gatewayId, peripheralDeviceId))
                {
                    return null!;
                }
                else
                {
                    return eGateway;
                }

            }

        }

        async Task<ActionResult<Gateway>?> GetGatewayBySerialNumber(string serialNumber)
        {
            using var c1SysAdminDBContext = _contextFactory.CreateDbContext();

            var eGateway = await c1SysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.SerialNumber == serialNumber);
            
            if (eGateway != null)
            {
                return eGateway;
            }
            return null!;
        }
        private async Task<Gateway> LoadPeripheralDevices(Gateway nGateway)
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext(); 
            var jAllPeripheralDevices = await cSysAdminDBContext.PeripheralDevice.ToListAsync();

            nGateway.LsPeripheralDevices = jAllPeripheralDevices
                .FindAll(iPeripheralDevices => iPeripheralDevices.GatewayID == nGateway.ID);

            //https://docs.microsoft.com/en-us/ef/core/querying/tracking  !!! To Study 
            //c1SysAdminDBContext.Entry(nGateway).Collection(x => x.LsPeripheralDevices).Load();
            //foreach (var jPeripheralDevice in nGateway.LsPeripheralDevices) 
            //{
            //    iGateway.LsPeripheralDevices.Add(jPeripheralDevice);
            //}
            return nGateway;
        }
        private async Task<bool> IsDeviceInGateway(int gatewayId, int peripheralDeviceId)
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();

            var eGateway = await cSysAdminDBContext.Gateway.FirstAsync(e => e.ID == gatewayId);

            using var c1SysAdminDBContext = _contextFactory.CreateDbContext();

            var ePeripheralDevice = await c1SysAdminDBContext.PeripheralDevice.FirstAsync(e => e.ID == peripheralDeviceId);

            if (eGateway == null || ePeripheralDevice == null)
            {
                throw new Exception("Error. Inexistent element.");
            }

            eGateway = await LoadPeripheralDevices(eGateway); 

            return eGateway.LsPeripheralDevices.Contains(ePeripheralDevice);
             
        }
        async Task<Gateway?> IGatewayRepository.GetGatewayBySerialNumber(string serialNumber)
        {
            using var cSysAdminDBContext = _contextFactory.CreateDbContext();
            if (cSysAdminDBContext.Gateway.ToListAsync().Result.Count > 0)
            {
                using var c1SysAdminDBContext = _contextFactory.CreateDbContext();
                Gateway? rGateway = await c1SysAdminDBContext.Gateway.FirstOrDefaultAsync(e => e.SerialNumber == serialNumber);

                if (rGateway != null)
                {
                    rGateway = LoadPeripheralDevices(rGateway).Result;
                    return rGateway;
                }
                else
                {
                    return null!;
                }
            }
            else
            {
                return null!;
            }

        }

    }
}
