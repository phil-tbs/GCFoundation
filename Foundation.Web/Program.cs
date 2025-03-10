using Foundation.Components.Middleware;
using cloudscribe.Web.Localization;
using cloudscribe.Web.SiteMap;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using Foundation.Web.Infrastructure.Extensions;
using Foundation.Components.Services.Interfaces;
using Foundation.Components.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
builder.Services.AddCloudscribeNavigation(builder.Configuration.GetSection("NavigationOptions"));

// Localization configuration
builder.Services.Configure<GlobalResourceOptions>(builder.Configuration.GetSection("GlobalResourceOptions"));
builder.Services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();
builder.Services.AddLocalization();

builder.Services.AddSingleton(typeof(IBreadcrumbsLocalizationService), typeof(BreadcrumbsLocalizationService<Foundation.Web.Resources.Navigation>));




var supportedCultures = new[] { new CultureInfo("en-CA"), new CultureInfo("fr-CA") };

var routeSegmentLocalizationProvider = new FirstUrlSegmentRequestCultureProvider(supportedCultures.ToList());

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture: "en-CA", uiCulture: "en-CA");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(0, routeSegmentLocalizationProvider);
});

// Add route localization using the custom extension method
builder.Services.AddCustomRouteLocalization();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<FoundationComponentsMiddleware>();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use localization middleware
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var culture = "en"; // Default culture
    var path = context.Request.Path.Value;

    // If the request is for the root ("/"), redirect to the default culture route
    if (string.IsNullOrEmpty(path) || path == "/")
    {
        context.Response.Redirect($"/{culture}/home/");
        return;
    }

    await next();
});

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}"
);


app.Run();
