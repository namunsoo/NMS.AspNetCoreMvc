using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NMS.AspNetCoreMvc.Web.DataContext;
using NMS.AspNetCoreMvc.Web.Models.Api;
using System.Linq;

namespace NMS.AspNetCoreMvc.Web.ApiControllers
{
    public class CommonApiController : Controller
    {
        private readonly CommonDB _commonDB;

        public CommonApiController(CommonDB commonDB)
        {
            _commonDB = commonDB;
        }

        [HttpPost]
        public async Task<IActionResult> IsIdAvailable([FromBody] CheckID request)
        {
            try
            {
                if (ModelState.IsValid) {
                    var user = await _commonDB.User.FirstOrDefaultAsync(c => c.ID.Equals(request.ID));
                    if (user == null)
                    {
                        return Json(true);
                    }
                    return Json(false);
                }
                return BadRequest("Enter required fields");
            } 
            catch 
            {
                return BadRequest("Error");
            }

        }

        [HttpPost]
        public IActionResult GetBoardList([FromBody] BoardApi request)
        {
            try
            {
				var table = _commonDB.Board.OrderByDescending(b => b.CreateDate).Where(b => true);
                int count = table.Count();
				#region [| 정렬 Logic |]
				if (request.Sort != null && request.Sort.Any())
                {
					string sortColumn = "CreateDate";
                    string sortType = "desc";
                    for (int i = 0; i < request.Sort.Count; i++)
                    {
                        sortColumn = request.Sort[i].Field;
                        sortType = request.Sort[i].Dir;
                        switch (sortColumn)
                        {
                            case "Category":
								table = sortType.Equals("desc") ? table.OrderByDescending(b => b.Category) : table.OrderBy(b => b.Category);
                                break;
                            case "Title":
								table = sortType.Equals("desc") ? table.OrderByDescending(b => b.Title) : table.OrderBy(b => b.Title);
                                break;
                            default:
								table = sortType.Equals("desc") ? table.OrderByDescending(b => b.CreateDate) : table.OrderBy(b => b.CreateDate);
                                break;
                        }
                    }
				}
				#endregion

				if (request.Filter != null)
				{
                    string value = request.Filter.Filters[0].Value;
					table = table.Where(b => b.Title.Contains(value) || b.Category.Contains(value) || b.CreateUserName.Contains(value));
					//for (int i = 0; i < request.Filter.Filters.Count; i++)
					//{
					//	switch (request.Filter.Filters[i].Field)
					//	{
					//		case "Title":
					//			table = table.Where(b => b.Title.Contains(request.Filter.Filters[i].Value));
					//			break;
					//		case "Category":
					//			table = table.Where(b => b.Category.Contains(request.Filter.Filters[i].Value));
					//			break;
					//		case "CreateUserName":
					//			table = table.Where(b => b.CreateUserName.Contains(request.Filter.Filters[i].Value));
					//			break;
					//		default:
					//			break;
					//	}
					//}
				}

				if (request.CategoryNo != 0)
                {
                    table = table.Where(b => b.Category.Equals(request.CategoryName));
				}
				var data = table.Skip(request.Skip).Take(request.Take).ToList();

                object returnData = new { Data = data, Total = table.Count() };
                return Json(returnData);
			}
            catch
            {
                return BadRequest("Error");
            }
        }

		[HttpPost]
		public IActionResult BoardCategory()
		{
			try
			{
				var data = _commonDB.BoardCategory.ToList();
				return Json(data);
			}
			catch
			{
				return BadRequest("Error");
			}
		}
	}
}
