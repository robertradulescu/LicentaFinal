using LicentaFinal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Areas.Identity.Data;
using LicentaFinal.Core.Repositories;
using LicentaFinal.Core;
using LicentaFinal.Core.ViewModels;
using LicentaFinal.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();




#region Authorization

AddAuthorizationPolicies();

#endregion

AddScoped();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "order-chart",
        pattern: "Orders/chart",
        defaults: new { controller = "Order", action = "Chart" });

    endpoints.MapControllerRoute(
       name: "exporttocsv",
       pattern: "{controller=OrderItems}/{action=ExportToCsv}");
});


app.MapControllerRoute(
    name: "DownloadInvoice",
    pattern: "Orders/DownloadInvoice/{id}",
    defaults: new { controller = "Orders", action = "DownloadInvoice" });

app.MapControllerRoute(
      name: "GenerateReceipt",
      pattern: "OrderItems/GenerateReceipt/{id}",
      defaults: new { controller = "OrderItems", action = "GenerateReceipt" });

app.MapControllerRoute(
     name: "orderhistory",
     pattern: "{controller=OrderHistory}/{action=Index}/{id?}");


app.MapRazorPages();




app.Run();


void AddAuthorizationPolicies()
{

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
        options.AddPolicy(Constants.Policies.RequireManager, policy => policy.RequireRole(Constants.Roles.Manager));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}
