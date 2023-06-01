using System;

using BD9.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;   // ������������ ���� ������ ApplicationContext
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//��� DateTime
var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/LogIn/Login";
    options.AccessDeniedPath = "/LogIn/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddIdentity<User, IdentityRole>(options =>//��������� ����, ���� ������ ��� �����
{
    options.Password.RequiredLength = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

var app = builder.Build();
app.UseStaticFiles();//�����������
app.UseRouting();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;


    var db = serviceProvider.GetRequiredService<ApplicationContext>();
    await db.Database.EnsureDeletedAsync();//��������� ��
    await db.Database.EnsureCreatedAsync();//cjplfybt ���� ������
    await ApplicationContextSeed.InitializeDb(db);//���������� ���� ������

}
app.Run();