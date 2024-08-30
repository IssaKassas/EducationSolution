using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CoursesModel
    {
        [Key]
        public int Id { get; set; }

        public string? name_course { get; set; }

        public string? image_course { get; set; }

        public double price { get; set; }

		public int number_users { get; set; }

		public int number_comments { get; set; }

        public string? name_instructor { get; set; }
	}
}
