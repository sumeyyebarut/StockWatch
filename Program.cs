using System;
using System.Configuration;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using StockWatch.Data;
using StockWatch.Filters;


var builder = WebApplication.CreateBuilder(args);

 // Add Hangfire services.
         builder.Services.AddHangfire(x => x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
         .UseSimpleAssemblyNameTypeSerializer()
         .UseDefaultTypeSerializer()
         .UseInMemoryStorage());
    builder.Services.AddHangfireServer();  // Hangfire iş server'ını ekle

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(); // Swagger 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Home/Error");
  
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
}
if (app.Environment.IsDevelopment())
{   app.UseSwagger();
    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseHangfireDashboard(); // 

app.MapHangfireDashboard("/hangfire", new DashboardOptions
                {         
                    AsyncAuthorization =  new[] { new AllowAllAsyncDashboardAuthorizationFilter() },
                    IgnoreAntiforgeryToken = true
                });



app.Run();
//dotnet ef dbcontext scaffold "Server=localhost;port=3306;Database=StockWatch;User=root;Password=Sumeyye1234;" Pomelo.EntityFrameworkCore.MySql --output-dir Models --context-dir Data --context AppDbContext --force
