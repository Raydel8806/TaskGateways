#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusalaGatewaysSysAdmin.Models;

namespace MusalaGatewaysSysAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewaysController : ControllerBase
    {
        private IGatewayRepository _gatewayRepository;

        public GatewaysController(IGatewayRepository gatewayRepository)
        {
            _gatewayRepository = gatewayRepository;
        }
        // METHOD: URL          using   HttpHeaders 'Content-Type': 'application/json;'
        // GET: api/Gateways                          1  OK
        [HttpGet]
        public IEnumerable<Gateway> GetAllGateway()
        {
            var gateways = _gatewayRepository.GetAllGateways();
            return gateways.ToArray(); 
        }

        // GET: api/Gateways/5                        2  OK
        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> GetGateway(int id)
        {
            try
            {
                var gateway = await _gatewayRepository.GetGateway(id);

                if (gateway != null)
                {
                    return gateway.Value;
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Gateways    {Gateway from body}   3  OK
        [HttpPut]
        public async Task<ActionResult<Gateway>> PutGateway(Gateway gateway)
        {
            try
            {
                if (gateway != null)
                {
                    if (gateway.SerialNumber != "")
                    {
                        Regex IPv4Format = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");

                        var isInDBGateway = await _gatewayRepository.GetGatewayBySerialNumber(gateway.SerialNumber);

                        if (isInDBGateway != null)
                        {
                            ModelState.AddModelError("SerialNumber", "Gateway Serial Number already in use.");
                            return BadRequest(ModelState);
                        }
                        else if (gateway.IpAddress != null)
                        {
                            if (!IPv4Format.IsMatch(gateway.IpAddress))
                            {
                                ModelState.AddModelError("ModelError", "IP Address format error.");

                                return BadRequest(ModelState);
                            }
                            else
                            if (gateway.LsPeripheralDevices != null && gateway.MaxClientNumber != null)
                            {
                                if (gateway.LsPeripheralDevices.Count > gateway.MaxClientNumber)
                                {
                                    ModelState.AddModelError("ModelError", "Max client number violation.");
                                    return BadRequest(ModelState);
                                }
                                else
                                {
                                    var eGateway = await _gatewayRepository.AddGateway(gateway);

                                    if (eGateway == null)
                                    {
                                        ModelState.AddModelError("ModelError", "Internal Server Error.");
                                        return BadRequest(ModelState);
                                    }
                                    else
                                    {
                                        return eGateway.Value;
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("NoData", "Empty fields.");
                                return BadRequest(ModelState);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("NoData", "Empty fields.");
                            return BadRequest(ModelState);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("SerialNumber", "Empty fields.");
                        return BadRequest(ModelState);

                    }
                }
                else
                {
                    ModelState.AddModelError("NoData", "Empty Object.");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            { 
                var eGateway = await _gatewayRepository.GetGateway(gateway.ID);
                if (eGateway != null)
                {
                    // The current values are the values that the application tried to write to the database.???
                    if (gateway == eGateway.Value)
                    { 
                        return eGateway;
                    }
                    else
                    {
                        throw new NotSupportedException(e.Message);
                    }
                }
                else
                {
                     return await PutGateway(gateway);
                }
            }

        }

        // PATCH: api/Gateways  {Gateway from body}    4  OK
        [HttpPatch]
        public async Task<ActionResult<Gateway>> UpdateGateway(Gateway gateway)
        {
            try
            {
                var eGateway = await _gatewayRepository.UpdateGateway(gateway);

                if (eGateway.Value == gateway) return eGateway;

                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            { 
                var eGateway = _gatewayRepository.GetGateway(gateway.ID).Result;
                if (eGateway.Value != null)
                {
                    // The current values are the values that the application tried to write to the database.???
                    if (gateway == eGateway.Value)
                    {
                        return eGateway;
                    }
                    else
                    { 
                        return await UpdateGateway(gateway);
                    }
                }
                else
                {
                    throw new NotSupportedException("Unknow Db Update Concurrency Exception");
                }
            }
            
        }
         
        // DELETE: api/Gateways/5                     5  OK          
        [HttpDelete("{id}")]
        public async Task<ActionResult<Gateway>> DeleteGateway(int id)
        {
            var eGateway = await _gatewayRepository.GetGateway(id);

            if (eGateway == null)
            {
                return NotFound();
            }

            var gateway = await _gatewayRepository.DeleteGateway(id);

            return gateway.Value;

        }

        // POST Periferical Device: api/Gateways/5     6  OK
        //      {PeripheralDevice from body}
        [HttpPost("{id=int}")]
        public async Task<ActionResult<Gateway>> AddPeriphericalDevice(int id, PeripheralDevice peripheralDevice)
        { 
            try
            {
                var eGateway = await _gatewayRepository.AddDeviceToGateway(id, peripheralDevice);

                if (eGateway == null)
                {
                    return NotFound();
                }

                return eGateway.Value;
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message.ToString());
            }
        }

        // DELETE: api/Gateways?{idGateway=#}&{idPDevice=#} 7 OK             
        [HttpDelete]
        public async Task<ActionResult<Gateway>> DeletePeriphericalDevice(int idGateway, int idPDevice)
        {     
            try
            {
                var eGateway = await _gatewayRepository.DeleteDeviceFromGateway(idGateway, idPDevice);

                if (eGateway == null)
                {
                    return NotFound();
                }

                return eGateway.Value;

            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message.ToString());
            }
        }

    } 
}
