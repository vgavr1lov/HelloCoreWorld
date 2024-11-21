using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

namespace HelloCoreWorld
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var builder = WebApplication.CreateBuilder(args);

         builder.WebHost.ConfigureKestrel(options =>
         {
            options.Listen(IPAddress.Loopback, 8088, listenOptions =>
            {
               listenOptions.UseHttps("testcertificate.pfx", "testing123");
            });
         });

         // Add services to the container.
         builder.Services.AddControllersWithViews();

         var app = builder.Build();

         // Configure the HTTP request pipeline.
         if (!app.Environment.IsDevelopment())
         {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseForwardedHeaders(new ForwardedHeadersOptions
         {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
         });

         app.UseHttpsRedirection();
         app.UseStaticFiles();

         app.UseRouting();

         app.UseAuthorization();

         app.MapControllerRoute(
             name: "default",
             pattern: "{controller=Home}/{action=Index}/{id?}");

         app.Run();
      }
   }
}
