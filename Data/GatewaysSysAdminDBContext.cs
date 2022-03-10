using Microsoft.EntityFrameworkCore; 

namespace MusalaGatewaysSysAdmin.Models
{
    /// <summary>
    /// This class allows us to query, create, edit and delete records 
    /// in a database with a high degree of abstraction together with
    /// the entity framework
    /// </summary>
    public class GatewaysSysAdminDBContext : DbContext
    {
        public GatewaysSysAdminDBContext(DbContextOptions<GatewaysSysAdminDBContext> options)
            : base(options)
        {
        }
        public DbSet<Gateway> Gateway { get; set; } = null!;
 
        public DbSet<PeripheralDevice> PeripheralDevice { get; set; }= null!; 
    }
} 