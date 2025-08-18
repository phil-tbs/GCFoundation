# GCFoundation AI Assistant Guide

This guide helps AI programming assistants understand and work with the GCFoundation libraries for building Government of Canada applications using ASP.NET Core and the GC Design System.

## 📋 Table of Contents

1. [Library Overview](#library-overview)
2. [Quick Setup](#quick-setup)
3. [Configuration Guide](#configuration-guide)
4. [Layout Template System](#layout-template-system)
5. [Component Catalog](#component-catalog)
6. [Common Patterns](#common-patterns)
7. [Security Features](#security-features)
8. [Troubleshooting](#troubleshooting)

## 🏗️ Library Overview

### GCFoundation.Components
**Purpose**: Main UI component library providing TagHelpers for the GC Design System (GCDS) and Foundation Data Control Panel (FDCP) components.

**Key Features**:
- GCDS TagHelpers (buttons, cards, forms, navigation, etc.)
- FDCP TagHelpers (data tables, advanced forms, modals, etc.)
- Automatic CSS/JS resource injection via middleware
- Bilingual support (English/French Canadian)
- Built-in validation and accessibility features
- Slot-based layout system for complex components

### GCFoundation.Common
**Purpose**: Shared utilities and settings across all GCFoundation libraries.

**Key Features**:
- Language utilities for Canadian bilingual applications
- JSON serialization helpers
- Shared configuration models
- Common enums and constants

### GCFoundation.Security
**Purpose**: Security middleware and policies for Government of Canada applications.

**Key Features**:
- Content Security Policy (CSP) middleware
- Security headers management
- CDN policy configuration
- HTTPS enforcement helpers

## ⚡ Quick Setup

### 1. Install Packages
```xml
<PackageReference Include="GCFoundation.Components" Version="1.0.0" />
<PackageReference Include="GCFoundation.Common" Version="1.0.0" />
<PackageReference Include="GCFoundation.Security" Version="1.0.0" />
```

### 2. Program.cs Configuration
```csharp
using GCFoundation.Components.Middleware;
using GCFoundation.Components.Services;
using GCFoundation.Components.Services.Interfaces;
using GCFoundation.Security.Middlewares;
using GCFoundation.Common.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Add GCFoundation services
builder.Services.AddGCFoundationComponents(builder.Configuration);
builder.Services.AddGCFoundationContentPolicies(builder.Configuration);
builder.Services.AddGCFoundationSession(builder.Configuration);

// Configure localization for Canadian bilingual apps
var supportedCultures = LanguageUtility.GetSupportedCulture();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture: "en-CA", uiCulture: "en-CA");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Use GCFoundation middleware (ORDER MATTERS!)
app.UseMiddleware<GCFoundationComponentsMiddleware>();      // Injects CSS/JS resources
app.UseMiddleware<GCFoundationContentPoliciesMiddleware>(); // Adds security headers
app.UseGCFoundationComponents();
app.UseGCFoundationContentPolicies();
app.UseGCFoundationSession();

// Configure localization
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

// Standard routing with culture support
app.MapControllerRoute(
    name: "default",
    pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}"
);
```

### 3. View Imports (_ViewImports.cshtml)
```csharp
@addTagHelper *, GCFoundation.Components
@using GCFoundation.Components.Enums
@using GCFoundation.Components.Models
@using GCFoundation.Common.Utilities
```

## ⚙️ Configuration Guide

### appsettings.json Structure
```json
{
  "FoundationComponentsSettings": {
    "ApplicationNameEn": "My Application",
    "ApplicationNameFr": "Mon Application", 
    "SupportLinkEn": "mailto:support@example.com",
    "SupportLinkFr": "mailto:support@example.com",
    "IncludeGCDSResources": true,
    "IncludeFontAwesome": true,
    "GCDSVersion": "0.35.0",
    "FontAwesomeVersion": "6.4.2",
    "GlobalCssFiles": [
      "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css",
      "/css/custom-styles.css"
    ],
    "GlobalJavaScriptFiles": [
      "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js",
      "/js/custom-scripts.js"
    ],
    "GlobalMetaTags": [
      "<meta name=\"description\" content=\"Application description\">",
      "<meta name=\"author\" content=\"TBS-SCT\">"
    ],
    "GlobalLinkTags": [
      "<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">"
    ]
  },
  "ContentPolicySettings": {
    "JavascriptCDN": ["https://cdn.jsdelivr.net"],
    "CssCDN": ["https://cdn.jsdelivr.net"], 
    "FontCDN": [],
    "CssCDNHash": []
  },
  "FoundationSession": {
    "SessionTimeout": 20,
    "ReminderTime": 5,
    "UseReminder": true,
    "UseSession": true,
    "RefreshURL": "/authentication/refresh",
    "LogoutURL": "/authentication/logout"
  }
}
```

## 🏗️ Layout Template System

### Foundation Layout Template Pattern (_FoundationLayout.cshtml)

The GCFoundation uses a sophisticated layout template that automatically handles resource injection, language switching, and slot-based content insertion:

```html
@using GCFoundation.Common.Settings
@using GCFoundation.Common.Utilities
@using GCFoundation.Components.Helpers
@using GCFoundation.Components.Setttings
@using GCFoundation.Components.Enums
@using Microsoft.AspNetCore.Routing
@using Microsoft.Extensions.Options
@using cloudscribe.Web.Navigation

@inject IOptions<GCFoundationComponentsSettings> settings;
@inject IOptions<GCFoundationSessionSetting> sessionSetting;

<!DOCTYPE html>
<html lang="@LanguageUtility.GetCurrentApplicationLanguage()">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    
    @* Global meta tags from configuration *@
    @foreach (var metaTag in settings.Value.GlobalMetaTags)
    {
        @Html.Raw(metaTag)
    }
    
    @await RenderSectionAsync("Metas", required: false)
    
    <title>@ViewData["Title"] - @settings.Value.ApplicationName</title>
    
    @* CSS Resources (automatically injected by middleware) *@
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    @{
        // Set up language switching
        RouteValueDictionary routeValueDictionary = new RouteValueDictionary(ViewContext.RouteData.Values);
        routeValueDictionary["culture"] = LanguageUtility.GetOppositeLangauge();
        Language currentLanguage = LanguageUtility.GetCurrentApplicationLanguage() == "fr" ? Language.fr : Language.en;
    }

    @* GCDS Header with Slot-Based Content *@
    <gcds-header 
        lang-href="@Url.RouteUrl(routeValueDictionary)" 
        skip-to-herf="#main-content" 
        lang="@currentLanguage">
        
        @{
            string? menuPartialViewName = ViewData["MenuPartialViewName"] as string;
            string? searchPartialViewName = ViewData["SearchPartialViewName"] as string;
            string? bannerPartialViewName = ViewData["BannerPartialViewName"] as string;
        }

        @* Banner Slot *@
        @if (!string.IsNullOrEmpty(bannerPartialViewName))
        {
            <div slot="banner">
                @await Html.PartialAsync(bannerPartialViewName)
            </div>
        }

        @* Menu Slot *@
        @if (!string.IsNullOrEmpty(menuPartialViewName))
        {
            <div slot="menu">
                @await Html.PartialAsync(menuPartialViewName)
            </div>
        }

        @* Search Slot *@
        @if (!string.IsNullOrEmpty(searchPartialViewName))
        {
            <div slot="search">
                @await Html.PartialAsync(searchPartialViewName)
            </div>
        }

        @* Breadcrumb Slot *@
        <div slot="breadcrumb">
            @await Component.InvokeAsync("Navigation", new { 
                viewName = "GCDSBreadcrumbs", 
                filterName = NamedNavigationFilters.Breadcrumbs, 
                startingNodeKey = "" 
            })
        </div>
    </gcds-header>

    @* Main Content Container *@
    <gcds-container id="main-content" size="xl" centered tag="main" main-container class="fdcp-mb-300">
        <partial name="_PageNotification" />
        @RenderBody()
    </gcds-container>

    @* Footer *@
    <gcds-footer display="full"></gcds-footer>

    @* Modal Section *@
    @await RenderSectionAsync("Modals", required: false)

    @* Session Management Modal *@
    @if (sessionSetting.Value.UseSession && sessionSetting.Value.UseReminder)
    {
        @await Html.PartialAsync("Modals/_ExtendSessionModal");
    }

    @* JavaScript Resources (automatically injected by middleware) *@
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### Using the Foundation Layout

In your views, specify which partial views to load for different slots:

```csharp
@{
    Layout = "_FoundationLayout";
    ViewData["Title"] = "My Page";
    ViewData["MenuPartialViewName"] = "_MyMenu";        // Optional: custom menu
    ViewData["SearchPartialViewName"] = "_MySearch";    // Optional: custom search
    ViewData["BannerPartialViewName"] = "_MyBanner";    // Optional: custom banner
}

<gcds-heading tag="h1">Welcome to My Page</gcds-heading>
<gcds-text>This is the main content.</gcds-text>
```

## 🧩 Component Catalog

### GCDS Header Component (Slot-Based)

The GCDS header is the cornerstone component with slot-based architecture:

```html
<!-- Basic Header -->
<gcds-header 
    lang-href="/fr/current-page" 
    skip-to-herf="#main-content" 
    lang="en"
    signature-has-link="true"
    signature-variant="colour">
    
    <!-- Banner Slot -->
    <div slot="banner">
        <gcds-notice type="info">Site maintenance scheduled</gcds-notice>
    </div>
    
    <!-- Menu Slot -->
    <div slot="menu">
        <gcds-top-nav label="Main navigation">
            <gcds-nav-link href="/about">About</gcds-nav-link>
            <gcds-nav-link href="/services">Services</gcds-nav-link>
        </gcds-top-nav>
    </div>
    
    <!-- Search Slot -->
    <div slot="search">
        <gcds-search action="/search" method="get" name="query"></gcds-search>
    </div>
    
    <!-- Breadcrumb Slot -->
    <div slot="breadcrumb">
        <gcds-breadcrumbs>
            <gcds-breadcrumbs-item href="/">Home</gcds-breadcrumbs-item>
            <gcds-breadcrumbs-item>Current Page</gcds-breadcrumbs-item>
        </gcds-breadcrumbs>
    </div>
</gcds-header>
```

**Header Attributes:**
- `lang-href`: URL for language toggle link
- `skip-to-herf`: URL for skip-to-content link (note: there's a typo in the property name)
- `lang`: Current language (`Language.en` or `Language.fr`)
- `signature-has-link`: Whether signature links to canada.ca (default: `true`)
- `signature-variant`: Style variant (`HeaderSignatureVariant.colour` or `HeaderSignatureVariant.white`)

### GCDS Basic Components

```html
<!-- Button -->
<gcds-button 
    type="submit" 
    size="regular" 
    button-id="submit-btn"
    name="submit"
    value="submit"
    disable="false">Submit Form</gcds-button>

<!-- Available button types: button, link, submit, reset -->
<!-- Available sizes: regular, small -->

<!-- Text/Heading -->
<gcds-text size="h1" text-role="primary" margin-bottom="400">Page Title</gcds-text>
<gcds-heading tag="h2" character-limit="true" margin-bottom="300">Section Heading</gcds-heading>

<!-- Container -->
<gcds-container 
    size="xl" 
    padding="300" 
    centered="true" 
    tag="section"
    main-container="true"
    border="false">
    Content here
</gcds-container>

<!-- Available sizes: xs, sm, md, lg, xl, full -->

<!-- Icon -->
<gcds-icon 
    name="home" 
    size="h3" 
    label="Home icon"
    margin-left="100"
    margin-right="100"></gcds-icon>

<!-- Available icons: info-circle, warning-triangle, exclamation-circle, checkmark-circle,
     chevron-left, chevron-right, chevron-up, chevron-down, close, download, email, external, phone, search -->
<!-- Available sizes: text-small, text, h1, h2, h3, h4, h5, h6 -->
```

### GCDS Navigation Components

```html
<!-- Breadcrumbs -->
<gcds-breadcrumbs>
    <gcds-breadcrumbs-item href="/">Home</gcds-breadcrumbs-item>
    <gcds-breadcrumbs-item href="/products">Products</gcds-breadcrumbs-item>
    <gcds-breadcrumbs-item>Current Page</gcds-breadcrumbs-item>
</gcds-breadcrumbs>

<!-- Top Navigation -->
<gcds-top-nav label="Main navigation" alignment="right" lang="en">
    <gcds-nav-link href="/about" current="false">About</gcds-nav-link>
    <gcds-nav-link href="/services" current="true">Services</gcds-nav-link>
</gcds-top-nav>

<!-- Side Navigation -->
<gcds-side-nav label="Section navigation">
    <gcds-nav-link href="/section1">Section 1</gcds-nav-link>
    <gcds-nav-link href="/section2">Section 2</gcds-nav-link>
</gcds-side-nav>

<!-- Search -->
<gcds-search 
    action="/search" 
    method="get" 
    name="query" 
    search-id="main-search"
    placeholder="Search canada.ca">
</gcds-search>
```

### GCDS Form Components

```html
<!-- Input (requires model binding with 'for' attribute) -->
<gcds-input 
    for="Model.Email"
    input-id="email" 
    label="Email Address" 
    autocomplete="off"
    hide-label="false">
</gcds-input>

<!-- Input without model binding (manual setup) -->
<gcds-input 
    input-id="email" 
    label="Email Address" 
    autocomplete="off"
    hide-label="false">
</gcds-input>

<!-- Available autocomplete: on, off -->

<!-- Select (requires model binding) -->
<gcds-select 
    for="Model.Province"
    select-id="province" 
    label="Province/Territory"
    default-value="">
    <option value="">Select a province</option>
    <option value="on">Ontario</option>
    <option value="qc">Quebec</option>
</gcds-select>

<!-- Textarea -->
<gcds-textarea 
    for="Model.Comments"
    textarea-id="comments" 
    label="Additional Comments" 
    rows="4"
    character-count="500"
    hide-label="false">
</gcds-textarea>

<!-- Checkboxes (with JSON options) -->
<gcds-checkboxes 
    for="Model.Notifications"
    legend="Notification Preferences" 
    name="notifications"
    options='[{"value":"email","text":"Email notifications"},{"value":"sms","text":"SMS notifications"}]'>
</gcds-checkboxes>

<!-- Radio Buttons (with JSON options) -->
<gcds-radios 
    for="Model.DeliveryMethod"
    legend="Delivery Method" 
    name="delivery"
    options='[{"value":"pickup","text":"Pickup"},{"value":"delivery","text":"Delivery"},{"value":"mail","text":"Mail"}]'>
</gcds-radios>

<!-- Note: Most GCDS form components inherit from BaseFormComponentTagHelper and support:
     - for="Model.Property" (model binding)
     - name, value, hint properties from base class
     - Automatic label and validation integration
-->
```

### GCDS Layout Components

```html
<!-- Grid System -->
<gcds-grid tag="div" columns="12" columns-desktop="12" columns-tablet="8" columns-mobile="4">
    <div>Column content</div>
</gcds-grid>

<!-- Card -->
<gcds-card 
    card-title="Service Information" 
    card-title-tag="h3"
    href="/services/details" 
    description="Learn more about our services"
    badge="New"
    img-src="/images/service.jpg"
    img-alt="Service illustration">
</gcds-card>

<!-- Details/Accordion -->
<gcds-details details-title="Additional Information">
    <gcds-text>This content is hidden by default and expands when clicked.</gcds-text>
</gcds-details>

<!-- Stepper -->
<gcds-stepper current-step="2" type="default">
    Step content here
</gcds-stepper>
```

### GCDS Feedback Components

```html
<!-- Notice/Alert -->
<gcds-notice 
    title="Success" 
    title-tag="h2"
    type="success">
    Your form has been submitted successfully.
</gcds-notice>

<!-- Available types: Success, Danger, Info, Warning -->
<!-- Available title-tag: h1, h2, h3, h4, h5, h6 -->

<!-- Error Summary -->
<gcds-error-summary>
    Please correct the following errors:
    <ul>
        <li><a href="#email">Email address is required</a></li>
        <li><a href="#phone">Phone number format is invalid</a></li>
    </ul>
</gcds-error-summary>

<!-- Error Message -->
<gcds-error-message>Please enter a valid email address</gcds-error-message>
```

### GCDS Footer

```html
<!-- Full Footer with contextual links -->
<gcds-footer 
    display="full"
    contextual-headling="Site Links"
    contextual-links='[{"label":"Contact Us","link":"/contact"},{"label":"Privacy","link":"/privacy"}]'
    sub-links='[{"label":"Terms","link":"/terms"},{"label":"Accessibility","link":"/accessibility"}]'>
</gcds-footer>

<!-- Compact Footer -->
<gcds-footer display="compact"></gcds-footer>

<!-- Available display types: full, compact -->
<!-- Links are passed as JSON objects with label and link properties -->
```

### GCDS Pagination

```html
<!-- List Pagination -->
<gcds-pagination 
    label="Navigation pagination"
    current-page="3"
    total-pages="10"
    display="List"
    previous-href="/page/2"
    previous-label="Previous"
    next-href="/page/4"
    next-label="Next"
    url="https://example.com/page/">
</gcds-pagination>

<!-- Simple Pagination -->
<gcds-pagination 
    label="Simple pagination"
    current-page="3"
    total-pages="10"
    display="Simple"
    previous-href="/page/2"
    next-href="/page/4">
</gcds-pagination>

<!-- Available display types: List, Simple -->
```

### FDCP Components (Advanced Form Components)

```html
<!-- FDCP Input with Model Binding -->
<fdcp-input for="User.Email"></fdcp-input>

<!-- FDCP Select with Options -->
<fdcp-select for="User.Province" options="ViewBag.ProvinceOptions"></fdcp-select>

<!-- FDCP Checkboxes with Model Binding -->
<fdcp-checkboxes for="User.Preferences" options="ViewBag.PreferenceOptions"></fdcp-checkboxes>

<!-- FDCP Radio Buttons -->
<fdcp-radios for="User.ContactMethod" options="ViewBag.ContactOptions"></fdcp-radios>

<!-- FDCP Form with Error Summary -->
<fdcp-form for="Model" method="post" action="/submit">
    <fdcp-error-summary></fdcp-error-summary>
    <!-- Form fields -->
    <gcds-button type="submit">Submit</gcds-button>
</fdcp-form>
```

### FDCP Data Components

```html
<!-- Tabulator Table -->
<fdcp-tabulator-table 
    table-id="dataTable" 
    data="Model.TableData"
    pagination="true"
    filter="true"
    sort="true"
    download="true"
    page-size="25">
</fdcp-tabulator-table>

<!-- Filters Box -->
<fdcp-filters-box title="Filter Options" filters="Model.FilterCategories">
</fdcp-filters-box>
```

### FDCP Layout Components

```html
<!-- Page Header -->
<fdcp-page-header 
    title="@Model.PageTitle" 
    subtitle="@Model.PageSubtitle"
    breadcrumbs="Model.Breadcrumbs">
</fdcp-page-header>

<!-- Badge -->
<fdcp-badge text="Status: Active" type="success" size="small"></fdcp-badge>

<!-- Stepper -->
<fdcp-stepper current-step="2" steps="Model.StepDefinitions"></fdcp-stepper>
```

### FDCP Modal Components

```html
<!-- Standard Modal -->
<fdcp-modal modal-id="confirmModal" title="Confirm Action">
    <fdcp-modal-body>
        <gcds-text>Are you sure you want to proceed with this action?</gcds-text>
    </fdcp-modal-body>
    <fdcp-modal-footer>
        <gcds-button type="button" onclick="closeModal()">Cancel</gcds-button>
        <gcds-button type="submit" onclick="confirmAction()">Confirm</gcds-button>
    </fdcp-modal-footer>
</fdcp-modal>

<!-- Session Management Modal (automatically included) -->
<fdcp-session-modal></fdcp-session-modal>
```

### FDCP Form Builder

```html
<!-- Dynamic Form Builder -->
<fdcp-form-builder form="Model.FormDefinition"></fdcp-form-builder>
```

## 🔄 Common Patterns

### Controller Pattern with Foundation Layout
```csharp
[Route("{culture:culture}/[controller]")]
public class ProductController : GCFoundationBaseController
{
    public IActionResult Index()
    {
        var model = new BaseViewModel
        {
            PageTitle = "Products",
            MetaDescription = "Browse our products"
        };
        
        // Set custom header components
        ViewData["MenuPartialViewName"] = "_ProductMenu";
        ViewData["SearchPartialViewName"] = "_ProductSearch";
        
        return View(model);
    }

    [HttpPost]
    public IActionResult Create(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Errors = ModelState.GetErrorSummary();
            return View(model);
        }
        
        // Process form
        TempData["SuccessMessage"] = "Product created successfully";
        return RedirectToAction("Index");
    }
}
```

### Model Pattern with Foundation
```csharp
public class ProductViewModel : BaseViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}
```

### Partial View for Header Slots
```html
@* _ProductMenu.cshtml *@
<gcds-top-nav label="Product navigation">
    <gcds-nav-link href="/products">All Products</gcds-nav-link>
    <gcds-nav-link href="/products/categories">Categories</gcds-nav-link>
    <gcds-nav-link href="/products/create">Add Product</gcds-nav-link>
</gcds-top-nav>

@* _ProductSearch.cshtml *@
<gcds-search 
    action="/products/search" 
    method="get" 
    name="query"
    placeholder="Search products...">
</gcds-search>
```

### Language Switching Pattern
```csharp
// In your layout or component
@{
    RouteValueDictionary routeValueDictionary = new RouteValueDictionary(ViewContext.RouteData.Values);
    routeValueDictionary["culture"] = LanguageUtility.GetOppositeLangauge();
    Language currentLanguage = LanguageUtility.GetCurrentApplicationLanguage() == "fr" ? Language.fr : Language.en;
}

<gcds-header lang-href="@Url.RouteUrl(routeValueDictionary)" lang="@currentLanguage">
    <!-- Header content -->
</gcds-header>
```

## 🔒 Security Features

### Content Security Policy
The security middleware automatically configures CSP headers based on your configuration:

```csharp
// Automatic CSP configuration includes:
// - script-src with nonce and allowed CDNs
// - style-src with hash validation and allowed CDNs
// - font-src for web fonts
// - img-src for images and data URIs
// - connect-src for AJAX requests
// - Localhost development allowances
```

### Security Headers
Automatically applied headers:
- Content-Security-Policy
- Strict-Transport-Security (HSTS)
- X-XSS-Protection
- X-Content-Type-Options (nosniff)
- Referrer-Policy
- Permissions-Policy
- Expect-CT
- Cache-Control

### Session Security Configuration
```csharp
// Configure secure session settings
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});
```

## 🚨 Troubleshooting

### Common Issues

#### 1. Header Slots Not Rendering
**Problem**: Content in header slots not appearing
**Solution**: 
- Ensure slot names are correct: `banner`, `menu`, `search`, `breadcrumb`
- Check that partial view names in ViewData are correct
- Verify partial views exist and are in the correct location

#### 2. Language Switching Not Working
**Problem**: Language toggle doesn't work
**Solution**:
- Verify route pattern includes `{culture=en}` parameter
- Check `RouteValueDictionary` setup in layout
- Ensure `LanguageUtility.GetOppositeLangauge()` is working

#### 3. CSS/JS Resources Not Loading
**Problem**: Foundation styles/scripts missing
**Solution**:
- Verify middleware order: `GCFoundationComponentsMiddleware` before `UseStaticFiles()`
- Check `FoundationComponentsSettings` configuration
- Ensure CDN URLs in configuration are accessible

#### 4. GCDS Components Not Styling
**Problem**: Components render but don't have proper GCDS styling
**Solution**:
- Verify `IncludeGCDSResources: true` in configuration
- Check that GCDS CSS/JS are loading from CDN
- Ensure no CSS conflicts with custom styles

#### 5. Breadcrumbs Not Appearing
**Problem**: Breadcrumb slot is empty
**Solution**:
- Check navigation.xml configuration
- Verify cloudscribe navigation is properly configured
- Ensure `Navigation` component is registered

#### 6. Session Modal Not Working
**Problem**: Session timeout modal not appearing
**Solution**:
- Verify `FoundationSession.UseSession: true` and `UseReminder: true`
- Check that `_ExtendSessionModal` partial exists
- Ensure session middleware is configured

#### 7. Model Validation Not Working
**Problem**: GCDS/FDCP form validation fails
**Solution**:
- Ensure model inherits from `BaseViewModel`
- Use proper `for="Model.Property"` binding in form components
- Check DataAnnotations are properly configured on model properties
- Verify ModelState is being checked in controller actions
- Use `required` attributes on TagHelper properties where needed

### Best Practices

1. **Always use the Foundation Layout** - `_FoundationLayout.cshtml` provides the complete GCDS experience
2. **Leverage slot-based architecture** - Use ViewData to specify custom partials for header slots
3. **Follow Government of Canada design guidelines** when customizing components
4. **Test bilingual functionality** - Verify French/English content renders correctly
5. **Use semantic HTML** - Components generate accessible markup by default
6. **Configure CSP carefully** - Add only necessary CDNs and sources
7. **Implement proper error handling** in controllers to populate error summaries
8. **Use FDCP components for data-heavy applications** - They include advanced features like filtering and pagination

### Performance Tips

1. **Enable response compression** in production
2. **Use CDN for static resources** when possible
3. **Minimize custom CSS/JS** - leverage built-in component styles
4. **Configure caching headers** for static assets
5. **Use async/await** for all I/O operations
6. **Implement proper error handling** to prevent resource leaks
7. **Consider lazy loading** for large datasets in tables

---

## 📚 Additional Resources

- [GC Design System Documentation](https://design-system.alpha.canada.ca/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Government of Canada Web Standards](https://www.canada.ca/en/treasury-board-secretariat/services/government-communications/canada-content-information-architecture-specification.html)

This guide provides AI assistants with comprehensive context for working with GCFoundation libraries, including the correct slot-based header implementation and layout template system.

