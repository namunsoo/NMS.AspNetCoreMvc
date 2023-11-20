using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NMS.AspNetCoreMvc.Web.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DB 연결 설정
builder.Services.AddDbContext<CommonDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommonDB") ?? throw new InvalidOperationException("Connection string 'CommonDB' not found.")));

// 쿠키 옵션
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // 쿠키 유지 시간
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home/Login"; // 쿠기 연결 거부됐을때 연결 경로 (에러페이지로 연결)
    });

// 쿠키 보안 정책
var cookiePolicyOptions = new CookiePolicyOptions
{
    // 다른 애플리케이션에서 접근하지 못하도록 하는거 같다.
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

// 세션 설정
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(20);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

builder.Services.AddHttpContextAccessor();

// asp net core mvc 에서 기본적으로 Json으로 보낼 때 대문자를 소문자로 변경하는데 이거 설정 막기
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
