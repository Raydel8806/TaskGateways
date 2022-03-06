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
    public interface IGatewayRepository
    {
        /// <summary>
        /// Return all existing Gateways in system.
        /// </summary>
        /// <returns>ActionResult<IEnumerable<Gateway>>></returns>
        IEnumerable<Gateway> GetAllGateways();

        Task<ActionResult<Gateway>> GetGateway(int gatewayId);
        
        Task<ActionResult<Gateway>> AddGateway(Gateway gateway);

        Task<ActionResult<Gateway>> DeleteGateway(int gatewayId);

        ActionResult<Gateway> UpdateGateway(Gateway gateway);

        Task<ActionResult<Gateway>> AddDeviceToGateway(int gatewayId, PeripheralDevice peripheralDevice);

        Task<ActionResult<Gateway>> DeleteDeviceFromGateway(int gatewayId, int peripheralDeviceId);
         
        Task<Gateway?> GetGatewayBySerialNumber(string serialNumber);
        //Task<bool> IsDeviceInGateway(int gatewayId, int peripheralDeviceId);

    }
}
