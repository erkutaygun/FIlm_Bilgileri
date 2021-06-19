using filmarsiv.data.Concrete.EfCore;
using filmarsiv.webui.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace filmarsiv.webui.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host){
            
            using (var scope = host.Services.CreateScope()){
                using(var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>()) {

                    try
                    {
                        applicationContext.Database.Migrate();
                    }
                    catch (System.Exception)
                    {
                        
                        throw;
                    }
                }
                using(var movieContext = scope.ServiceProvider.GetRequiredService<MovieContext>()) {

                    try
                    {
                        movieContext.Database.Migrate();
                    }
                    catch (System.Exception)
                    {
                        
                        throw;
                    }
                }
            }

            return host;
        }
    }
}