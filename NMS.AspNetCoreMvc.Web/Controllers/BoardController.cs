using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using NMS.AspNetCoreMvc.Web.DataContext;
using NMS.AspNetCoreMvc.Web.Models.DB;
using System;
using System.Security.Claims;
using static System.Formats.Asn1.AsnWriter;

namespace NMS.AspNetCoreMvc.Web.Controllers
{
    public class BoardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CommonDB _commonDB;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BoardController(ILogger<HomeController> logger, CommonDB commonDB, IHttpContextAccessor httpContextAccessor)
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
                ViewData["UserCode"] = UserCode;
            }
            else
            {
                ViewData["IsLogin"] = "false";
                ViewData["UserCode"] = "00000000-0000-0000-0000-000000000000";
            }
        }

        #region [| 게시글 목록 |]
        public IActionResult BoardList()
        {
            //var bb = _commonDB.Board.ToList().Skip(1).Take(4);

            return View();
        }
        #endregion

        #region [| 게시글 쓰기 |]
        public IActionResult BoardWrite()
        {
            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            UserCode = UserCode != null && !UserCode.Equals("00000000-0000-0000-0000-000000000000") ? UserCode : null;
            if (string.IsNullOrEmpty(UserCode))
            {
                TempData["RouteController"] = "Board";
                TempData["RouteAction"] = "BoardWrite";
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost, ActionName("BoardWrite")]
        [Authorize(Roles = "User")] // 필터 (쿠키접근 권한 확인)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoardWritePost([Bind("Category,Title,BoardContent")] Board board)
        {
            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            UserCode = UserCode != null && !UserCode.Equals("00000000-0000-0000-0000-000000000000") ? UserCode : null;

            if (!string.IsNullOrEmpty(UserCode))
            {
                var userData = _commonDB.User.Where(c => c.UserCode.Equals(new Guid(UserCode)));
                if (userData.ToList().Count != 0 && !string.IsNullOrEmpty(board.Category) && !string.IsNullOrEmpty(board.Title) && !string.IsNullOrEmpty(board.BoardContent))
                {
                    User user = userData.ToList()[0];
                    board.CreateUserCode = user.UserCode;
                    board.CreateUserName = user.Name;
                    board.CreateDate = DateTime.Now;
                    _commonDB.Update(board);
                    await _commonDB.SaveChangesAsync();
                }
                return RedirectToAction("BoardList", "Board");
            }
            return NotFound(); // 정상작동 오류 처리(alert창 등등) 또는 오류처리 페이지로 연결해야함
        }
        #endregion

        #region [| 게시글 상세보기 |]
        public IActionResult BoardDetail(int? No)
        {
            if (No == null || No == 0)
            {
                return RedirectToAction("Main", "Home");
            }
            var board = _commonDB.Board.FirstOrDefault(b => b.No == No);
            return View(board);
        }
        #endregion

        #region [| 게시글 수정 |]
        [Authorize(Roles = "User")] // 필터 (쿠키접근 권한 확인)
        public IActionResult BoardRetouch(int? No)
        {
            if (No == null || No == 0)
            {
                return RedirectToAction("Main", "Home");
            }

            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            var board = _commonDB.Board.FirstOrDefault(b => b.No == No);
            if (UserCode != null && board != null && board.CreateUserCode.Equals(new Guid(UserCode)))
            {
                return View(board);
            }
            return RedirectToAction("Main", "Home");
        }

        [HttpPost, ActionName("BoardRetouch")]
        [Authorize(Roles = "User")] // 필터 (쿠키접근 권한 확인)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoardRetouchPost(int? No, [Bind("No,Category,Title,BoardContent")] Board board)
        {
            if (No == null || No == 0 || No != board.No)
            {
                return RedirectToAction("Main", "Home");
            }

            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            UserCode = UserCode != null && !UserCode.Equals("00000000-0000-0000-0000-000000000000") ? UserCode : null;

            if (!string.IsNullOrEmpty(UserCode))
            {
                Guid updateUserCode = new Guid(UserCode);
                var userData = await _commonDB.User.FirstOrDefaultAsync(u => u.UserCode.Equals(new Guid(UserCode)));
                var boardToUpdate = await _commonDB.Board.FirstOrDefaultAsync(b => b.No == No && b.CreateUserCode.Equals(updateUserCode));
                if (userData != null && boardToUpdate != null)
                {
                    boardToUpdate.Category = board.Category;
                    boardToUpdate.Title = board.Title;
                    boardToUpdate.BoardContent = board.BoardContent;
                    boardToUpdate.UpdateUserCode = userData.UserCode;
                    boardToUpdate.UpdateUserName = userData.Name;
                    boardToUpdate.UpdateDate = DateTime.Now;
                    if (await TryUpdateModelAsync<Board>(boardToUpdate, "", b => b.Category, b => b.Title, b => b.BoardContent, b => b.UpdateUserCode, b => b.UpdateUserName, b => b.UpdateDate))
                    {
                        try
                        {
                            await _commonDB.SaveChangesAsync();
                        }
                        catch (DbUpdateException /* ex */)
                        {
                            // 에러처리 로직
                            //Log the error (uncomment ex variable name and write a log.)
                            ModelState.AddModelError("", "Unable to save changes. " +
                                "Try again, and if the problem persists, " +
                                "see your system administrator.");
                        }
                        return RedirectToAction("BoardList", "Board");
                    }
                }
            }
            return NotFound(); // 정상작동 오류 처리(alert창 등등) 또는 오류처리 페이지로 연결해야함
        }
        #endregion

        #region [| 게시글 삭제 |]
        [Authorize(Roles = "User")] // 필터 (쿠키접근 권한 확인)
        public async Task<IActionResult> BoardDelete(int? No)
        {
            if (No == null || No == 0)
            {
                return RedirectToAction("Main", "Home");
            }

            string? UserCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserCode");
            var board = await _commonDB.Board.FirstOrDefaultAsync(b => b.No == No);
            if (UserCode != null && board != null && board.CreateUserCode.Equals(new Guid(UserCode)))
            {
                _commonDB.Board.Remove(board);
                await _commonDB.SaveChangesAsync();
                return RedirectToAction("BoardList", "Board");
            }
            return NotFound(); // 정상작동 오류 처리(alert창 등등) 또는 오류처리 페이지로 연결해야함
        }
        #endregion
    }
}
