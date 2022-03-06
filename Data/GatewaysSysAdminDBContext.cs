using Microsoft.EntityFrameworkCore; 

namespace MusalaGatewaysSysAdmin.Models
{
    public class GatewaysSysAdminDBContext : DbContext
    {
        public GatewaysSysAdminDBContext(DbContextOptions<GatewaysSysAdminDBContext> options)
            : base(options)
        {
        }
        public DbSet<Gateway> Gateway { get; set; }
 
        public DbSet<PeripheralDevice> PeripheralDevice { get; set; }
         
        
    }
}
/*
 
  IConfigurationRoot configuration = new ConfigurationBuilder()
       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
       .AddJsonFile("appsettings.json")
       .Build();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("GatewaysDB"));
        }*/