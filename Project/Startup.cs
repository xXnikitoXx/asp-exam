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
using Project.Hubs;
using Project.Services.PayPal;
using Project.Services.Hetzner;

namespace Project {
	public class Startup {
		public Startup(IConfiguration configuration) =>
			Configuration = configuration;

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(0));

			MapperConfiguration mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new ApplicationProfile()));
			services.AddSingleton(mapperConfig.CreateMapper());
			services.AddSingleton<IUserStatusClient, UserStatusClient>();
			services.AddSingleton<PayPalPaymentClient>();
			
			services.AddScoped<IAccountClient, AccountClient>();
			services.AddScoped<IAdminClient, AdminClient>();
			services.AddScoped<IAnnouncementClient, AnnouncementClient>();
			services.AddScoped<IMessageClient, MessageClient>();
			services.AddScoped<IOrderClient, OrderClient>();
			services.AddScoped<IPaymentClient, PaymentClient>();
			services.AddScoped<IPlanClient, PlanClient>();
			services.AddScoped<IPromoCodeClient, PromoCodeClient>();
			services.AddScoped<IRoleClient, RoleClient>();
			services.AddScoped<ITicketClient, TicketClient>();
			services.AddScoped<IVPSClient, VPSClient>();
			services.AddScoped<IServerClient, HetznerServerClient>();

			services.AddScoped<MessageHub>();

			services.AddAntiforgery();
			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddSignalR();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context) {
			if (env.IsDevelopment()) {
				context.Database.EnsureCreated();
				app.UseDeveloperExceptionPage();
			} else {
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

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}"
				);
				endpoints.MapControllerRoute(
					name: "orderList",
					pattern: "Order/List{Page=1}&{Show=20}"
				);
				endpoints.MapRazorPages();
				endpoints.MapHub<MessageHub>("/Messages");
			});
		}
	}
}
