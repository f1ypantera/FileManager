using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using FileManagerDBLogic.Context;
using FileManagerDBLogic.ConnectionSettings;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Services;
using FileManagerBussinessLogic.Interfaces;
using FileManagerBussinessLogic.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System;
using FileManagerBussinessLogic.Socket;

namespace FileManagerAPI
{

    public  class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => 
                {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<Settings>(
              options =>
              {
                  options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                  options.Database = Configuration.GetSection("MongoDb:Database").Value;
                  options.Container = Configuration.GetSection("MongoDb:Container").Value;
                  options.IsContained = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
              });

            services.AddTransient<IMongoContext, MongoContext>();
            services.AddTransient<IAccountMongoService, AccountMongoService>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddTransient<ITimerAlarm, TimerAlarm>();
            services.AddTransient<ITestService, TestService>();
            services.AddSingleton<FileSocketManager>();
            services.AddAutoMapper();


            
            
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

     
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
              
                app.UseHsts();
            }
      
            app.UseSwagger();
  

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseHttpsRedirection();

            app.UseWebSockets();
            app.UseMiddleware<SocketMiddleware>();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }

    }
}

