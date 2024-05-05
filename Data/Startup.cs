using Microsoft.EntityFrameworkCore;

namespace Order_Management.Data
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add session services
            services.AddSession();
            // Register ApplicationDbContext with the dependency injection container
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DConnection"))
            );

            

            // Other service registrations...
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession(); // Enable session middleware
            // Configure middleware and pipeline...
        }
    }
}
