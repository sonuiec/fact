using FactFinderWeb.Models;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
	.AddEnvironmentVariables();
//call connection string


 
// Add services to the container.{our project}
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<AdminUserServices>();
builder.Services.AddScoped<AwarenessServices>();
builder.Services.AddScoped<WingsServices>();
builder.Services.AddScoped<KnowledgeThatMattersServices>();
builder.Services.AddScoped<ExecutionServices>();
builder.Services.AddScoped<AlertnessMappingService>();
builder.Services.AddScoped<InvestServices>();
builder.Services.AddScoped<JSONDataUtility>();
builder.Services.AddScoped<UtilityHelperServices>();
builder.Services.AddDbContext<ResellerBoyinawebFactFinderWebContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("FactFinderDbCon")));

builder.Services.AddHttpContextAccessor();
//builder.Services.AddSession(); // for session support

//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// Add services to the container.
builder.Services.AddControllersWithViews();
 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();


var cultureInfo = new CultureInfo("en-US"); // Base for number format (12,345.67)
cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; // Force dd/MM/yyyy format
cultureInfo.DateTimeFormat.DateSeparator = "/";

// Apply globally
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

if (app.Environment.IsDevelopment())
{
    // Detailed errors for local debugging only
    app.UseDeveloperExceptionPage();
}
else
{
    // Friendly error page + security in Production/Staging
    app.UseExceptionHandler("/Error"); // <-- point to a controller/action or Razor page
    app.UseHsts();
}


app.UseCors("AllowAll");

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"❌ Global Error: {ex}"); // will show in logs/console
//        throw; // rethrow so you still see 500 in browser
//    }
//});


app.Run();


/*   
 
 four plan have same struc and always same?

 
 https://awaken-server-production-fd4f.up.railway.app/api/awaken/create-form

https://localhost:7010/Comprehensive/KnowledgeThatMatters   this value change dynamicly pls share some details
Overall risk profile
Cautious Aggressive







AWARENESS - Applicant details, spouse det, 

wings - goals
Alertness -- Applicant's Details, assets , libilites

Knowledge That Matters --- Risk 
Execution With Precision   --- 

Invest  ---- Action plan for Financial Goals






var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WSignageContext>();
//builder.Services.AddTransient<IUsers, UsersService>(); 

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);

app.Run();

 */






