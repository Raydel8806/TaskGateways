using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusalaGatewaysSysAdmin.Models
{
    public class PeripheralDevice 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
         
        [Required]
        [Display(Name = "User ID")]
        public long UId { get; set; }

        [Required]
        [Display(Name = "Device Vendor")]
        public string? DeviceVendor { get; set; }

        [Required]
        [Display(Name = "Device Created Date")]
        public DateTime DtDeviceCreated{ get; set; }

        [Required]
        [Display(Name = "Estatus")]
        public bool Online { get; set; }
          
        public int GatewayID { get; set; }

    }
}
