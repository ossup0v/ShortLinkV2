using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShortLinkAppV2._0.BusinessLogic;
using ShortLinkAppV2._0.Configs;
using ShortLinkAppV2._0.Interfaces;

namespace ShortLinkAppV2._0
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
            services.AddTransient<ILinkStorage, LinkStorage>();
            services.AddTransient<ILinkService, LinkService>();
            services.AddControllers();
            services.AddMvcCore(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerConfig = new SwaggerConfig();
            Configuration.GetSection(nameof(swaggerConfig)).Bind(swaggerConfig);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerConfig.JsonRoute;
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerConfig.UIEndpoint, swaggerConfig.Description);
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Link}/{action=index}/{id?}");
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
