using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMS.AspNetCoreMvc.Web.Models.DB
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserCode { get; set; }
        public string? Name { get; set; }
        public string? ID { get; set; }
        public string? Password { get; set; }
    }

    public class QuestionCards
    {
        [Key]
        public int No { get; set; }
        public Guid UserCode { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public int Importance { get; set; }
        public bool Memorize { get; set; }
    }

    public class Board
    {
        [Key]
        public int No { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? BoardContent { get; set; }
        public Guid CreateUserCode { get; set; }
        public string? CreateUserName { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? UpdateUserCode { get; set; }
        public string? UpdateUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

	public class BoardCategory
	{
		[Key]
		public int CategoryNo { get; set; }
		public string CategoryName { get; set; }
	}
}
