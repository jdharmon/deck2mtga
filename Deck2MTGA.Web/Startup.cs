using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deck2MTGA.Web.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;

namespace Deck2MTGA.Web
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
            services.AddMvc(config => 
                config.Conventions.Add(new ApiActionModelConvention())
            );
            services.AddSwaggerGen(config => 
            {
                config.SwaggerDoc("v1", new Info() { Title = "MTG Arena Deck Converter", Version = "v1"});
            });
            services.AddMemoryCache();
            services.AddSingleton<IMtgDbContext>(new MtgDbContext());
            services.AddScoped<ICardRepository>(provider =>
                new CardRepository(provider.GetService<IMtgDbContext>())
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var pathBase = Configuration["PathBase"];
            if (!string.IsNullOrEmpty(pathBase))
                app.UsePathBase(pathBase);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Deck Converter V1"));
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
