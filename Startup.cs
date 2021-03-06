﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using CityInfo.API.Services;
using Microsoft.Extensions.Configuration;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API
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
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
            //adding Entity Framework Service

            //adding a repository pattern , with this apporach mock a service will be easy
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
            //adding a repository pattern, with this apporach mock a service will be easy
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
                CityInfoContext cityInfoContext)
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

            //use for seeding
            cityInfoContext.EnsureSeedDataForContext();
            //use for seeding

            //which will print stus page in html format
            app.UseStatusCodePages();
            //which will print stus page in html format
            
            //Use Auto Mapper
            AutoMapper.Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDTO>();
                    cfg.CreateMap<Entities.City, Models.CityDto>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                    cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
                    cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
                }
            );
            //Use Auto Mapper

            //enable use js and css files and icons
            app.UseStaticFiles();
            //enable use js and css files and icons

            //use MVC
            app.UseMvc(
                routes => 
                {
                    routes.MapRoute
                    (
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}"
                    );
                } 
            
            );





            //use MVC


            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }
    }
}
