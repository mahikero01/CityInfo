using CityInfo.API.Services;
using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace CityInfo
{
    public class Startup
    {
        //this is use for configuration injection also instatitate this in the constructor
        //this configuration is using appSettings.json
        public static IConfiguration Configuration { get; private set; }
        //this is use for configuration injection also instatitate this in the constructor
        //this configuration is using appSettings.json

        public Startup(IConfiguration configuration) //IHostingEnvironment env
        {
            //implementation of the configuration
            // var builder = new ConfigurationBuilder()
            //         .SetBasePath(env.ContentRootPath)
            //         .AddJsonFile("appSettings.json", optional:false, reloadOnChange:true);
            // Configuration = builder.Build();
            //implementation of the configuration

            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddMvcOptions(o => o.OutputFormatters.Add(
                //This option will add XML output formatter option
                new XmlDataContractSerializerOutputFormatter()
            ));
            

            //THis is only use if you want the properties in JSON are Sentenced Case
            // .AddJsonOptions(o => {
            //     if (o.SerializerSettings.ContractResolver != null) 
            //     {
            //         var castedResolver = o.SerializerSettings.ContractResolver
            //                 as DefaultContractResolver;
            //             castedResolver.NamingStrategy = null;
            //     }
            // });

            //adding a customized service
            //this use in Staging
            services.AddTransient<IMailService, LocalMailService>();

            //this is use to Prod
            //services.AddTransient<IMailService, CloudMailService>();
            //adding a customized service

            //adding Entity Framework Service
            //var connectionString = @"Server=B04SQLD50\JHEADEV8R2; Database=dbbtCARSAp1; Trusted_Connection=True";
            var connectionString = @"Data Source=B04SQLD50\JHEADEV8R2; Initial Catalog=dbbtCARSAp1; Integrated Security=True";
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
            //adding Entity Framework Service
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //logging system configuration
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();
            ConfigureExtensions.ConfigureNLog(loggerFactory, "../../../nlog.config");
            //logging system configuration

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }else 
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseMvc();


            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }
    }
}
