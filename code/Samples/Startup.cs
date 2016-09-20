using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Samples.DI;
using Samples.Middleware;
using Samples.Models;
using Microsoft.EntityFrameworkCore;

namespace Samples
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile("appsettings1.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets();
			}

			builder.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//configure ef context
			var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;";
			services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connection));

			// Add framework services.
			services.AddMvc();

			// Add functionality to inject IOptions<T>
			services.AddOptions();

			// Add our Config object so it can be injected
			services.Configure<AsdfConfig>(Configuration.GetSection("AppSettings"));

			//Depedency Injection
			services.AddTransient<IEmailSender, EmailSender>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMiddleware<RequestLoggerMiddleware>();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

		}
	}
}
