using DigitalSignService.Interfaces;
using DigitalSignService.Services;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace DesktopService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            });
            builder.Services.AddTransient<IDMSWrapper, DMSWrapper>();
            builder.Services.AddSingleton<IAppSettingService, AppSettingService>();
            builder.Services.AddScoped<ICheckOutService, CheckOutService>();
            //builder.Services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            //});
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            builder.Services.AddAntiforgery(x =>
            {
                x.SuppressXFrameOptionsHeader = true;
            });


            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
                app.UseDeveloperExceptionPage();


            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}