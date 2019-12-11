﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
                    cfg.User.RequireUniqueEmail = true
                )
                .AddEntityFrameworkStores<DutchContext>();
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer();
            services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
            });
            //The below code might work; but inorder to bullet proof it, 
            //we need to understand that DutchSeeder actually contains scoped dependencies.
            //How do we know this? => In the Startup class, services.AddDbContext<DutchSeeder> creates it as Scoped dependency
            services.AddTransient<DutchSeeder>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());            
            services.AddScoped<IDutchRepository, DutchRepository>();
            services.AddTransient<INullMailService, NullMailService>();
            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseStaticFiles();
            app.UseNodeModules(env);
            //app.UseRouting();
            //app.UseEndpoints();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=App}/{action=Index}/{id?}");
            });
        }
    }
}
