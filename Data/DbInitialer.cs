namespace GatewaysSysAdminWebAPI.Data
{
    /// <summary>
    /// This class initializes the database for the first time.
    /// </summary>
    public class DbInitialer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var servicesScope = applicationBuilder.ApplicationServices.CreateScope())
            { 

            } 

        }
    }
}
