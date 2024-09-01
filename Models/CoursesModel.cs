using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CoursesModel
    {
        [Key]
        public int Id { get; set; }

        public string? course_name { get; set; }

        public string? course_image { get; set; }

        public double price { get; set; }

		public int number_users { get; set; }

		public int number_comments { get; set; }

        public string? instructor_name { get; set; }

        public int category_id { get; set; }
	}
}
