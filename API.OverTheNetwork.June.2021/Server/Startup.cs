using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using ShareInvest.Hubs;

namespace ShareInvest
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;
		public IConfiguration Configuration
		{
			get;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSignalR();
			services.AddResponseCompression(o => o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }));
			services.AddRazorPages();
			services.Configure<KestrelServerOptions>(o =>
			{
				o.ListenAnyIP(7135);
				o.Limits.MaxRequestBodySize = int.MaxValue;
			})
				.AddTransient<BalanceHub>()
				.AddTransient<MessageHub>()
				.AddControllersWithViews(o => o.InputFormatters.Insert(0, GetJsonPatchInputformatter()))
				.AddMvcOptions(o => o.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);
			Base.SendMessage(GetType().Name, services.GetType());
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseMvc().UseDeveloperExceptionPage().UseWebAssemblyDebugging();

			else
			{
				app.UseMvc().UseExceptionHandler("/Error");
				Base.SendMessage(app.GetType().Name, env.GetType());
			}
			app.UseBlazorFrameworkFiles().UseStaticFiles().UseRouting().UseEndpoints(ep =>
			{
				ep.MapRazorPages();
				ep.MapControllers();
				ep.MapHub<MessageHub>("/hub/message");
				ep.MapHub<BalanceHub>("/hub/balance");
				ep.MapFallbackToFile("index.html");
			});
		}
		static NewtonsoftJsonInputFormatter GetJsonPatchInputformatter()
			=> new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
			.Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
	}
}