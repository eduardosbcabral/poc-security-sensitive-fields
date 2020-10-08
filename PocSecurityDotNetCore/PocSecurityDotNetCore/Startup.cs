using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PocSecurityDotNetCore.Models;
using PocSecurityDotNetCore.Services;
using PocSecurityDotNetCore.Http;

namespace PocSecurityDotNetCore
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
            services.AddSingleton<ICipherService, RijndaelCipherService>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserQueryModel>();
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDataProtection();

            services.AddControllers()
                .UseCgdfSecurity(Configuration.GetValue<string>("Security_secret"));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
