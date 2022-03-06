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

        // PUT: api/Gateways                          3  OK
        // Gateway from body
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754                         
        [HttpPut]
        public async Task<ActionResult<Gateway>> PutGateway(Gateway gateway)
        {
            try
            {
                if (gateway != null)
                {
                    if (gateway.SerialNumber != null)
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
                                ModelState.AddModelError("IpAddress", "IP Address format error.");

                                return BadRequest(ModelState);
                            }
                            else
                            if (gateway.LsPeripheralDevices != null && gateway.MaxClientNumber != null)
                            {
                                if (gateway.LsPeripheralDevices.Count > gateway.MaxClientNumber)
                                {
                                    ModelState.AddModelError("MaxClientNumber", "Max client number violation.");
                                    return BadRequest(ModelState);
                                }
                                else
                                {
                                    var eGateway = await _gatewayRepository.AddGateway(gateway);

                                    if (eGateway == null)
                                    {
                                        return NotFound();
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
                        ModelState.AddModelError("NoData", "Empty fields.");
                        return BadRequest(ModelState);

                    }
                }
                else
                {
                    ModelState.AddModelError("NoData", "Empty Object.");
                    return BadRequest(ModelState);
                }
            }
            catch (DbUpdateConcurrencyException DbEx)
            {
                if (await _gatewayRepository.GetGateway(gateway.ID) == null)
                {
                    return NotFound();
                }
                else
                {
                    return NotFound(DbEx.Message);
                }
            }

        }

        // UPDATE: api/Gateways                       4  OK
        [HttpPatch]
        public ActionResult<Gateway> UpdateGateway(Gateway gateway)
        {
            var eGateway = _gatewayRepository.UpdateGateway(gateway);

            if (eGateway == null) return NotFound();

            return eGateway.Value;
        }

        /*       
       // POST: api/Gateways
       // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       [HttpPost]
       public async Task<ActionResult<Gateway>> PostGateway(Gateway gateway)
       { 

           _context.Gateway.Add(gateway);
           await _context.SaveChangesAsync();

           return CreatedAtAction("GetGateway", new { id = gateway.ID }, gateway);
       }
        */
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

        // ADD Periferical Device: api/Gateways/5     6  OK
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

        // DELETE: api/Gateways/5                           
        //                                             7 OK             
        [HttpDelete]
        public async Task<ActionResult<Gateway>> DeletePeriphericalDevice(int idGateway, int idPDevice)
        {   //Tratamiento de Execciones

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


    /*
    public class GatewaysController2 : Controller
    {
        private readonly GatewaysSysAdminDBContext _context;

        public GatewaysController(GatewaysSysAdminDBContext context)
        {
            _context = context;
        }

        // GET: Gateways
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gateway.ToListAsync());
        }

        // GET: Gateways/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gateway = await _context.Gateway
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gateway == null)
            {
                return NotFound();
            }

            return View(gateway);
        }

        // GET: Gateways/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gateways/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SerialNumber,Name,IpAddress,MaxClientNumber")] Gateway gateway)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gateway);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gateway);
        }

        // GET: Gateways/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gateway = await _context.Gateway.FindAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }
            return View(gateway);
        }

        // POST: Gateways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SerialNumber,Name,IpAddress,MaxClientNumber")] Gateway gateway)
        {
            if (id != gateway.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gateway);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatewayExists(gateway.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gateway);
        }

        // GET: Gateways/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gateway = await _context.Gateway
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gateway == null)
            {
                return NotFound();
            }

            return View(gateway);
        }

        // POST: Gateways/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gateway = await _context.Gateway.FindAsync(id);
            _context.Gateway.Remove(gateway);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GatewayExists(int id)
        {
            return _context.Gateway.Any(e => e.ID == id);
        }
    }
    */
}
