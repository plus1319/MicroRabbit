using System;
using System.IO;
using System.Reflection;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MicroRabbit.Transfer.Api
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
            services.AddDbContext<TransferDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("TransferDbConnection")));

            services.AddControllers();

            services.AddMediatR(typeof(Startup).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer Microservice", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
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


            #region swagger config
            //please add into project
            //<PropertyGroup >
            //  <GenerateDocumentationFile>true</GenerateDocumentationFile>
            //  <NoWarn>$(NoWarn);1591</NoWarn>
            //</PropertyGroup>
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice");
                c.RoutePrefix = string.Empty;
            });
            #endregion

            app.UseAuthorization();

            ConfigureEventBust(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
          
        }
        //subscribe event 
        private void ConfigureEventBust(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
        }
    }
}
