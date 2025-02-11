using Foundation.Components.Middleware;
using cloudscribe.Web.Localization;
using cloudscribe.Web.SiteMap;
using Microsoft.Extensions.Configuration;
using cloudscribe.Web.Navigation;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
builder.Services.AddCloudscribeNavigation(builder.Configuration.GetSection("NavigationOptions"));

builder.Services.Configure<GlobalResourceOptions>(builder.Configuration.GetSection("GlobalResourceOptions"));
builder.Services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { new CultureInfo("en-CA"), new CultureInfo("fr-CA") };


var routeSegmentLocalizationProvider = new FirstUrlSegmentRequestCultureProvider(supportedCultures.ToList());

builder.Services.Configure<RequestLocalizationOptions>(options =>
{


    // State what the default culture for your application is. This will be used if no specific culture
    // can be determined for a given request.
    options.DefaultRequestCulture = new RequestCulture(culture: "en-CA", uiCulture: "en-CA");

    // You must explicitly state which cultures your application supports.
    // These are the cultures the app supports for formatting numbers, dates, etc.
    options.SupportedCultures = supportedCultures;

    // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
    options.SupportedUICultures = supportedCultures;

    // You can change which providers are configured to determine the culture for requests, or even add a custom
    // provider with your own logic. The providers will be asked in order to provide a culture for each request,
    // and the first to provide a non-null result that is in the configured supported cultures list will be used.
    // By default, the following built-in providers are configured:
    // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
    // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
    // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
    //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
    //{
    //  // My custom request culture logic
    //  return new ProviderCultureResult("en");
    //}));

    options.RequestCultureProviders.Insert(0, routeSegmentLocalizationProvider);

});



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<FoundationComponentsMiddleware>();

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



app.UseEndpoints(endpoints => 
{
    endpoints.MapControllerRoute(
        name: "default-localized",
        pattern: "{culture}/{controller}/{action}/{id?}",
        defaults: new { controller = "Home", action = "Index" },
        constraints: new { culture = new CultureSegmentRouteConstraint() }
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
