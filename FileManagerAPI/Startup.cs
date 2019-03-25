using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using FileManagerDBLogic.Context;
using FileManagerDBLogic.ConnectionSettings;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Services;
using FileManagerBussinessLogic.Interfaces;
using FileManagerBussinessLogic.Infrastructure;
using FileManagerDBLogic.Models;

namespace FileManagerAPI
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => 
                {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            string con = "Data Source=KBP1-LHP-F76802\\SQLEXPRESS;Initial Catalog=FileManager_API;MultipleActiveResultSets=true;User Id=Admin;Password = Admin";
            services.AddDbContext<MSSQLContext>(options => options.UseSqlServer(con));


            services.Configure<Settings>(
              options =>
              {
                  options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                  options.Database = Configuration.GetSection("MongoDb:Database").Value;
              });

            services.AddTransient<IMongoContext, MongoContext>();
            services.AddTransient<IRepositoryMongoService, RepositoryMongoService>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddTransient<ITimerAlarm, TimerAlarm>();
            services.AddTransient<IRepositoryMSSQLService<Owner>, RepositoryMSSQLService<Owner>>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

     
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseWebSockets();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseHttpsRedirection();
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
