
using CIBnext.BLL;
using CIBnext.DAL;
using CIBnext.DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Login";
    opt.LogoutPath = "/Logout";
    opt.Cookie.Name = ".cibnext";
    opt.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<AccountDAL>();
builder.Services.AddScoped<AccountBLL>();

builder.Services.AddScoped<MiscDAL>();
builder.Services.AddScoped<MiscBLL>();

builder.Services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CIBnext.DAL.ListOfValuesDAL>();
builder.Services.AddScoped<CIBnext.BLL.ListOfValuesBLL>();
builder.Services.AddScoped<CIBnext.DAL.SubjectDAL>();
builder.Services.AddScoped<CIBnext.BLL.SubjectBLL>();
builder.Services.AddScoped<CIBnext.BLL.MisBLL>();
builder.Services.AddScoped<CIBnext.BLL.ReportBLL>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();