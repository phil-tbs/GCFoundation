using cloudscribe.Web.Localization;
using cloudscribe.Web.SiteMap;
using GCFoundation.Common.Utilities;
using GCFoundation.Components.Middleware;
using GCFoundation.Components.Services;
using GCFoundation.Components.Services.Interfaces;
using GCFoundation.Security.Middlewares;
using GCFoundation.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

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

// Configure breacrumbds localization service
builder.Services.AddSingleton(typeof(IBreadcrumbsLocalizationService), typeof(BreadcrumbsLocalizationService<GCFoundation.Web.Resources.Navigation>));

// Configure foundation

builder.Services.AddGCFoundationComponents(builder.Configuration);
builder.Services.AddGCFoundationContentPolicies(builder.Configuration);
builder.Services.AddGCFoundationSession(builder.Configuration);

// Language configuration
var supportedCultures = LanguageUtility.GetSupportedCulture();

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

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/Shared/Components/Navigation/{0}.cshtml");
    options.ViewLocationFormats.Add("/contentFiles/any/net8.0/Views/Shared/Components/Navigation/{0}.cshtml");
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Load all javascript dependencies for foundation and GCDS
app.UseMiddleware<GCFoundationComponentsMiddleware>();

// Add foundation security middleware(Add CSP)
app.UseMiddleware<GCFoundationContentPoliciesMiddleware>();

app.UseMiddleware<GCFoundationLanguageMiddleware>();

// Secure Cookies
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,  // Prevent cross-site requests
    Secure = CookieSecurePolicy.Always,  // Only send cookies over HTTPS
    HttpOnly = HttpOnlyPolicy.Always  // Prevent JavaScript access to cookies
});

// Use Foundation
app.UseGCFoundationComponents();
app.UseGCFoundationContentPolicies();
app.UseGCFoundationSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use localization middleware
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}"
);


app.Run();
