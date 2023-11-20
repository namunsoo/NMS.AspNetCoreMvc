using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NMS.AspNetCoreMvc.Web.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DB ���� ����
builder.Services.AddDbContext<CommonDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommonDB") ?? throw new InvalidOperationException("Connection string 'CommonDB' not found.")));

// ��Ű �ɼ�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // ��Ű ���� �ð�
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home/Login"; // ��� ���� �źε����� ���� ��� (������������ ����)
    });

// ��Ű ���� ��å
var cookiePolicyOptions = new CookiePolicyOptions
{
    // �ٸ� ���ø����̼ǿ��� �������� ���ϵ��� �ϴ°� ����.
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

// ���� ����
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(20);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

builder.Services.AddHttpContextAccessor();

// asp net core mvc ���� �⺻������ Json���� ���� �� �빮�ڸ� �ҹ��ڷ� �����ϴµ� �̰� ���� ����
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseCookiePolicy(cookiePolicyOptions);
//app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/");
app.Run();
