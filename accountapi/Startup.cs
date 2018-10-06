using System;
using accountapi.Actors;
using accountapi.Contents;
using accountapi.Controllers;
using accountapi.Repository;
using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using accountapi.Config;

using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace accountapi
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
            String dbstr = Configuration.GetConnectionString("db_account")
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {           
            services.AddDbContext<AccountContent>(opt => {
                opt.UseLoggerFactory(LogSettings.ConsoleLogger);
                opt.UseMySql(Configuration.GetConnectionString("db_account"));
            });

            services.AddSingleton<ActorSystem>(_ => ActorSystem.Create("accountapi"));

            //services.AddSingleton<AccountService>();
            services.AddTransient<AccountService>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            app.UseStaticFiles();

            if ( env.IsDevelopment() )
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<AccountContent>();
                    //context.Database.EnsureDeleted();
                    //context.Database.EnsureCreated();
                    var actorSystem = serviceScope.ServiceProvider.GetRequiredService<ActorSystem>();
                    System.Console.WriteLine( "Actor System Check==="+actorSystem.Name);

 
                }
                app.UseDeveloperExceptionPage();               
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.RoutePrefix = string.Empty;
                // Set the comments path for the Swagger JSON and UI.

            });

            app.UseSession();
            app.UseMvc();
        }
    }
}
