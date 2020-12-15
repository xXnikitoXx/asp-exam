using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Models;
using Project.Services.Native;
using AutoMapper;
using Project.MappingConfiguration;
using System;

namespace Project
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
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(0));

			MapperConfiguration mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new ApplicationProfile()));
			services.AddSingleton(mapperConfig.CreateMapper());
			
			services.AddScoped<IRoleClient, RoleClient>();
			services.AddScoped<IAccountClient, AccountClient>();
			services.AddScoped<IAdminClient, AdminClient>();
			services.AddScoped<IOrderClient, OrderClient>();
			services.AddScoped<IVPSClient, VPSClient>();
			services.AddScoped<IPlanClient, PlanClient>();
			services.AddScoped<IOrderClient, OrderClient>();
			services.AddScoped<IPromoCodeClient, PromoCodeClient>();
			services.AddScoped<IAnnouncementClient, AnnouncementClient>();

			services.AddAntiforgery();
			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
		{
			if (env.IsDevelopment())
			{
				context.Database.EnsureCreated();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// app.UseHsts();
			}

			app.Use(async (context, next) => {
				await next();
				if (context.Response.StatusCode == 404)
				{
					context.Request.Path = "/404";
					await next();
				}
			});

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}"
				);
				endpoints.MapControllerRoute(
					name: "orderList",
					pattern: "Order/List{Page=1}&{Show=20}"
				);
				endpoints.MapRazorPages();
			});
		}
	}
}
