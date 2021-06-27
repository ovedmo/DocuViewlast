using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using GdPicture14.WEB;
using System.Reflection;
using DocuViewareBlazor.Server.EventDispatcher;

namespace DocuViewareBlazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var path = System.IO.Path.GetDirectoryName(assemblyPath);
            DocuViewareLicensing.RegisterKEY("0404403347189896460401864");
            DocuViewareManager.SetupConfiguration(true, DocuViewareSessionStateMode.InProc,
                path + "\\Cache",
                "https://localhost:44393/", "api/DocuViewareREST");

            DocuViewareEventsHandler.PageTransferReady += DocuViewareEventDispatcher.PageTransferReady;
            DocuViewareEventsHandler.NewDocumentLoaded += DocuViewareEventDispatcher.NewDocumentLoaded;
            DocuViewareEventsHandler.LoadDocumentError += DocuViewareEventDispatcher.LoadDocumentError;
            DocuViewareEventsHandler.CustomAction += DocuViewareEventDispatcher.CustomAction;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
