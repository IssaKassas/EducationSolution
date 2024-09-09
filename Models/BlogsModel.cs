namespace WebApp.Models
{
	public class BlogsModel
	{
		public int Blog_Id { get; set; }

		public string? instructor_name { get; set; }

		public DateTime dob { get; set; }

		public int number_comments { get; set;  }

		public string? title { get; set; }

		public string? blog_text { get; set; }

		public string? blog_image { get; set; }
	}
}
