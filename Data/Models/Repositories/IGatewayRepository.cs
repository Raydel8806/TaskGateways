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
    /// <summary>
    /// The IGatewayRepository interface is created to attract the 
    /// REST Service from the data storage technology to be used.
    /// </summary>
    public interface IGatewayRepository
    {
        /// <summary>
        /// Return all existing Gateways in system.
        /// </summary>
        /// <returns>ActionResult<IEnumerable<Gateway>>></returns>
        IEnumerable<Gateway> GetAllGateways();

        /// <summary>
        /// Method that has the functionality of returning a 
        /// Gateway object if it exists, receiving its identifier as a parameter.
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        Task<ActionResult<Gateway>> GetGateway(int gatewayId);

        /// <summary>
        /// Method that stores a new gateway in the system.
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        Task<ActionResult<Gateway>> AddGateway(Gateway gateway);

        /// <summary>
        /// Method that eliminates a Gateway receiving its identifier as a parameter
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        Task<ActionResult<Gateway>> DeleteGateway(int gatewayId);

        /// <summary>
        /// method to update a Gateway
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        Task<ActionResult<Gateway>> UpdateGateway(Gateway gateway);

        /// <summary>
        /// Method that receives by parameter a Gateway 
        /// id and a new peripheral device and adds this device to the gateway.
        /// It has as a precondition that the Gateway has the capacity to register it.
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="peripheralDevice"></param>
        /// <returns></returns>
        Task<ActionResult<Gateway>> AddDeviceToGateway(int gatewayId, PeripheralDevice peripheralDevice);

        /// <summary>
        /// Method to remove a peripheral device from the Gateway
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="peripheralDeviceId"></param>
        /// <returns></returns>
        Task<ActionResult<Gateway>> DeleteDeviceFromGateway(int gatewayId, int peripheralDeviceId);

        /// <summary>
        /// Method that returns a Gateway receiving its serial number as a parameter.
        /// As the serial number is a unique attribute, it is necessary to validate 
        /// that it does not exist in the system.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        Task<Gateway?> GetGatewayBySerialNumber(string serialNumber);
         
    }
}
