using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyQueue.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyQueue
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddDbContext<MQDBContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("MQDBContext")));
            services.AddIdentityCore<IdentityUser>(
                opt => 
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 8;
                    opt.User.RequireUniqueEmail = true;
                    opt.SignIn.RequireConfirmedEmail = false;
                }
                ).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MQDBContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
