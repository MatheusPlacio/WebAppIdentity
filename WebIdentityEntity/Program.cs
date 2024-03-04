using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebAppIdentityEntity.Models;
using WebIdentityEntity.Data;
using WebIdentityEntity.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

// Conexão com o banco de dados (Sql Server)
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
                 builder.Services.AddDbContext<MyContext>(options =>
                                                                  options.UseSqlServer(connection,
                                                                  x => x.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddIdentity<MyUser, IdentityRole>(options =>
                 {
                     options.SignIn.RequireConfirmedEmail = true;
                     options.Password.RequireDigit = false;
                     options.Password.RequireNonAlphanumeric = false;
                     options.Password.RequireLowercase = false;
                     options.Password.RequireUppercase = false;
                     options.Password.RequiredLength = 4;
                 })
                .AddEntityFrameworkStores<MyContext>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<NaoContemValidadorSenha<MyUser>>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<MyUser>, MyUserClaimsPrincipalFactory>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(3));

//builder.Services.AddAuthentication("cookies").AddCookie("cookies", options => options.LoginPath = "/User/Login");
builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/User/Login");

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()));

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

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
