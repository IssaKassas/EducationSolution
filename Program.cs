using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebApp;
Console.WriteLine(111);
var builder = WebApplication.CreateBuilder(args);

var Configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json")
		.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
		.AddEnvironmentVariables()
		.Build();

builder.Services?.AddSingleton<IBlockListService, BlockListService>();
builder.Services?.AddMemoryCache();
builder.Services?.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
builder.Services?.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
builder.Services?.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services?.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services?.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services?.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

builder.Services?.Configure<FormOptions>(o =>
{
	o.ValueLengthLimit = 1073741824;
	o.MultipartBodyLengthLimit = 1073741824;
	o.MemoryBufferThreshold = 1073741824;
	o.ValueCountLimit = 1073741824;
	o.BufferBodyLengthLimit = 1073741824;
});

builder.Services?.AddHsts(options =>
{
	// Set the maximum age (in seconds) that the browser should remember to enforce HSTS.
	options.MaxAge = TimeSpan.FromDays(365);

	// Specify whether to include subdomains in the HSTS policy.
	options.IncludeSubDomains = true;

	// Specify whether to preload the HSTS policy, which means including it in the browser's preload list.
	options.Preload = true;
});

builder.Services?.AddAntiforgery(options =>
{
	options.HeaderName = "X-CSRF-TOKEN";
});

builder.Services?.AddAuthorization(options =>
{
	options.AddPolicy("Approved", policy =>
	policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "true"
	));
});

builder.Services?.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromHours(2);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services?.ConfigureApplicationCookie(options =>
{
	options.Cookie.Name = "AspNetCore.Identity.Application";
	options.ExpireTimeSpan = TimeSpan.FromMinutes(200);
	options.SlidingExpiration = true;
});

builder.Services?.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/Account/login";
	options.AccessDeniedPath = "/Account/Logout";
	options.Cookie.IsEssential = true;
	options.SlidingExpiration = true;
	options.ExpireTimeSpan = TimeSpan.FromHours(2);
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

//builder.Services?.AddKendo();
//builder.Services?.AddDbContext<ApplicationDbContext>(options =>
//{
//	options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
//});

//builder.Services?.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//	options.SignIn.RequireConfirmedAccount = true;
//})
//.AddEntityFrameworkStores<ApplicationDbContext>()
//.AddDefaultTokenProviders();

// Add services to the container.
builder.Services?.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
