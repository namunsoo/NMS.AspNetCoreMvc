using System.ComponentModel.DataAnnotations;

namespace NMS.AspNetCoreMvc.Web.Models.Api
{
    public class CheckID
    {
        [Required]
        public string? ID { get; set; }
    }

    public class BoardApi
    {
        public int CategoryNo { get; set; }
        public string? CategoryName { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
		public List<KendoGridSort>? Sort { get; set; }
		public KendoGridFilter? Filter { get; set; }
	}

	public class KendoGridSort
	{
		public string Field { get; set; }
		public string Dir { get; set; }
	}

	public class KendoGridFilter
	{
		public List<KendoGridFilterDescription>? Filters { get; set; }
		public string? Logic { get; set; }
	}

	public class KendoGridFilterDescription
	{
		public string @operator { get; set; }
		public string Field { get; set; }
		public string Value { get; set; }
	}
}
