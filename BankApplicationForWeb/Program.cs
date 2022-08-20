using BankApplicationForWeb.Data;
using Microsoft.EntityFrameworkCore;
using BankApplicationForWeb.Models;
using BankApplicationForWeb.BillPayBackgroundService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<McbaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(McbaContext)));

    // Enable lazy loading.
    options.UseLazyLoadingProxies();
});

// Store session into Web-Server memory.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Make the session cookie essential.
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<RealBankService>();
builder.Services.AddHostedService<BillPayBackgroundService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed data.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        seedClass.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapDefaultControllerRoute();
app.Run();
