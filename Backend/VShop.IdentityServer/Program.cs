using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VShop.IdentityServer;
using VShop.IdentityServer.Configuration;
using VShop.IdentityServer.Data;
using VShop.IdentityServer.SeedDatabase;

var builder = WebApplication.CreateBuilder(args);

const string VARIABLE_PASSWORD = "ACCESS";
EnviromentFile.Load(enviromentFilePath: Path.Combine(Directory.GetCurrentDirectory(), ".env"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var mySqlConnection = builder.Configuration.GetConnectionString("Default")?.Replace("#PASSWORD#", Environment.GetEnvironmentVariable(VARIABLE_PASSWORD));

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var builderIdentityServer = builder.Services
    .AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityConfiguration.Clients)
    .AddAspNetIdentity<ApplicationUser>();

builderIdentityServer.AddDeveloperSigningCredential();

builder.Services.AddScoped<IDatabaseSeedInitializer, DatabaseIdentityServerInitializer>();

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
app.UseIdentityServer();

app.UseAuthorization();

SeedDatabaseIdentityServer(app: app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabaseIdentityServer(IApplicationBuilder app)
{
    using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
    {
        IDatabaseSeedInitializer? initRolesUsers = serviceScope.ServiceProvider.GetService<IDatabaseSeedInitializer>();

        initRolesUsers?.InitializeSeedRoles();
        initRolesUsers?.InitializeSeedUsers();
    }
}