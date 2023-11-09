using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.AspNetCoreMvc.Web.Models;
using System.Diagnostics;
using System.Security.Claims;
using NMS.AspNetCoreMvc.Web.DataContext;
using Microsoft.EntityFrameworkCore;
using NMS.AspNetCoreMvc.Web.Models.DB;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace NMS.AspNetCoreMvc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CommonDB _commonDB;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, CommonDB commonDB, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _commonDB = commonDB;
            _httpContextAccessor = httpContextAccessor;
        }

        // 액션 실행됐을때 공통 작업
        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            UserCode = UserCode != null && !UserCode.Equals("00000000-0000-0000-0000-000000000000") ? UserCode : null;
            if (!string.IsNullOrEmpty(UserCode))
            {
                ViewData["IsLogin"] = "true";
            }
            else
            {
                ViewData["IsLogin"] = "false";
            }
        }

        #region [| 로그인 |]
        public async Task<IActionResult> Login(User user)
        {
            if (!string.IsNullOrEmpty(user.ID) && !string.IsNullOrEmpty(user.Password))
            {
                var userData = await _commonDB.User.FirstOrDefaultAsync(u => u.ID.Equals(user.ID) && u.Password.Equals(user.Password));
                if (userData != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("UserCode", userData.UserCode.ToString()), // 값 설정
                        new Claim(ClaimTypes.Role, "User") // 쿠키접근 권한
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    string controller = HttpContext.Request.Query["RouteController"].ToString();
                    string action = HttpContext.Request.Query["RouteAction"].ToString();

                    // 로그인에 성공 
                    return RedirectToAction(action, controller);
                }
                else 
                {
                    // 로그인 실패
                    ModelState.AddModelError("Error", "로그인에 실패하였습니다.");
                    return View(); 
                }
            }
            return View();
        }
        #endregion

        #region [| 로그아웃 |]

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }
        #endregion

        #region [| 회원가입 |]
        public IActionResult Join()
        {
            return View();
        }

        [HttpPost, ActionName("Join")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinPost([Bind("Name,ID,Password")] User user)
        {
            if (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.ID) && !string.IsNullOrEmpty(user.Password))
            {
                try
                {
                    _commonDB.Update(user);
                    await _commonDB.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_commonDB.User?.Any(u => u.UserCode.Equals(user.UserCode))).GetValueOrDefault())
                    {
                        return NotFound(); // 에러페이지로 연결(이미 사용자가 존재한다)
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Login", "Home");
            }
            return NotFound(); // 정상작동 오류 처리(alert창 등등) 또는 오류처리 페이지로 연결해야함
        }
        #endregion

        #region [| 내 정보 |]
        [Authorize(Roles = "User")] // 필터 (쿠키접근 권한 확인)
        public IActionResult MyInfo()
        {
            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            UserCode = UserCode != null && !UserCode.Equals("00000000-0000-0000-0000-000000000000") ? UserCode : null;

            if (!string.IsNullOrEmpty(UserCode))
            {
                var userData = _commonDB.User.Where(u => u.UserCode.Equals(new Guid(UserCode)));
                User? user = userData.ToList().Count == 0 ? null : userData.ToList()[0];
                return View(user);
            }
            return NotFound(); // 정상작동 오류 처리(alert창 등등) 또는 오류처리 페이지로 연결해야함
        }
        #endregion

        #region [| Main |]
        public IActionResult Main()
        {
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}