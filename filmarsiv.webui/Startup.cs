using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using filmarsiv.business.Abstract;
using filmarsiv.business.Concrete;
using filmarsiv.data.Abstract;
using filmarsiv.data.Concrete.EfCore;
using filmarsiv.webui.EmailServices;
using filmarsiv.webui.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace filmarsiv.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration){
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options=> options.UseMySql(_configuration.GetConnectionString("MysqlConnection")));
            services.AddDbContext<MovieContext>(options=> options.UseMySql(_configuration.GetConnectionString("MysqlConnection")));
            
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options => {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;

                //Lockout
                options.Lockout.MaxFailedAccessAttempts = 4;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".MovieApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });
            

            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService,CategoryManager>();

            services.AddScoped<IMovieRepository,EfCoreMovieRepository>();
            services.AddScoped<IMovieService,MovieManager>();

            services.AddScoped<IEmailSender,SmtpEmailSender>(i=>
                new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                    _configuration.GetValue<int>("EmailSender:Port"),
                    _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:UserName"],
                    _configuration["EmailSender:password"]
                    )
                );
            
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles(); //wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"                
            });

            
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapControllerRoute(
                    name:"adminuseredit",
                    pattern:"admin/user/{id?}",
                    defaults: new {controller="Admin",action="UserEdit"}
                );
                endpoints.MapControllerRoute(
                    name:"adminusers",
                    pattern:"admin/user/list",
                    defaults: new {controller="Admin",action="UserList"}
                );
            
                endpoints.MapControllerRoute(
                    name:"adminroles",
                    pattern:"admin/role/list",
                    defaults: new {controller="Admin",action="RoleList"}
                );
                endpoints.MapControllerRoute(
                    name:"adminrolecreate",
                    pattern:"admin/role/create",
                    defaults: new {controller="Admin",action="RoleCreate"}
                );
                endpoints.MapControllerRoute(
                    name:"adminroleedit",
                    pattern:"admin/role/{id?}",
                    defaults: new {controller="Admin",action="RoleEdit"}
                );
                endpoints.MapControllerRoute(
                    name:"adminmovies",
                    pattern:"admin/movies",
                    defaults: new {controller="Admin",action="MovieList"}
                );
                endpoints.MapControllerRoute(
                    name:"adminmoviecreate",
                    pattern:"admin/movies/create",
                    defaults: new {controller="Admin",action="MovieCreate"}
                );
                endpoints.MapControllerRoute(
                    name:"adminmovieedit",
                    pattern:"admin/movies/{id?}",
                    defaults: new {controller="Admin",action="MovieEdit"}
                );
                endpoints.MapControllerRoute(
                    name:"admincategories",
                    pattern:"admin/categories",
                    defaults: new {controller="Admin",action="CategoryList"}
                );
                endpoints.MapControllerRoute(
                    name:"admincategorycreate",
                    pattern:"admin/categories/create",
                    defaults: new {controller="Admin",action="CategoryCreate"}
                );
                 endpoints.MapControllerRoute(
                    name:"admincategoryedit",
                    pattern:"admin/categories/{id?}",
                    defaults: new {controller="Admin",action="CategoryEdit"}
                );
               
                endpoints.MapControllerRoute(
                    name:"search",
                    pattern:"search",
                    defaults: new {controller="Movie",action="search"}
                );
                endpoints.MapControllerRoute(
                   name:"moviedetails",
                   pattern:"{url}",
                   defaults: new {controller="Movie",action="details"}
               );
                endpoints.MapControllerRoute(
                   name:"movies",
                   pattern:"movie/{category?}",
                   defaults: new {controller="Movie",action="list"}
               );
               endpoints.MapControllerRoute(
                   name:"movies",
                   pattern:"movies",
                   defaults: new {controller="Movie",action="list"}
               );
                endpoints.MapControllerRoute(
                   name:"default",
                   pattern:"{controller=Home}/{action=Index}/{id?}"
               );
            });

            SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        }
    }
}   
