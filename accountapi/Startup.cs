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

namespace accountapi
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddDbContext<AccountContent>(opt => {
                opt.UseMySql("server=localhost;database=db_account;user=psmon;password=db1234");
            });

            services.AddSingleton<ActorSystem>(_ => ActorSystem.Create("accountapi"));

            services.AddSingleton<AccountService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
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
            app.UseMvc();
        }
    }
}
