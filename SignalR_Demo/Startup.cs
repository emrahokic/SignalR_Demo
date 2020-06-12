using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using SignalR_Demo.SignalR;
using SignalR_Demo.Tasks;

namespace SignalR_Demo
{
    public class Startup
    {

        private IScheduler _scheduler;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _scheduler = GetScheduler();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR();
            
            services.AddSingleton(provider => _scheduler);
            services.AddScoped<IJob,SenzorTask>();

        }
        private void OnShutdown()
        {

            if (!_scheduler.IsShutdown)
                _scheduler.Shutdown();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSignalR(c =>
            {

                c.MapHub<SensorHub>("/sensor");
            });
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
        private IScheduler GetScheduler()
        {
            var props = new NameValueCollection
            {
                { "quartz.serializer.type","binary"}

            };

            var schedulerFactory = new StdSchedulerFactory(props);
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.Start().Wait();
            return scheduler;
        }
    }
}
