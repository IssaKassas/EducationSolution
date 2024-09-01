using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class RepositoryCourses
	{
		[Table("dtbl_courses")]
		public class Model
		{
			[Key]
			public int Id { get; set; }

			public string? instructor_name { get; set; }

			public string? course_image { get; set; }

			public double price { get; set; }

			public int instructor_id { get; set; }

			public int number_users { get; set; }

			public int number_comments { get; set; }

			public int category_id { get; set; }
		}

		public static List<CoursesModel?>? GetCourses()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<CoursesModel?>? result = null;

			try
			{
				using (var conn = new MySqlConnection(Program.ConnectionString))
				{
					var sql = @"
						SELECT * FROM dtbl_courses AS dc
						INNER JOIN dtbl_instructors AS di 
						ON dc.instructor_id = di.Id;
					";

					result = conn.Query<CoursesModel?>(sql).ToList();
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}

		public static List<CoursesModel?>? GetPopular()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<CoursesModel?>? result = null;

			try
			{
				using (var conn = new MySqlConnection(Program.ConnectionString))
				{
					var sql = @"
						SELECT * 
						FROM dtbl_courses AS dc
						INNER JOIN dtbl_instructors AS di 
						ON dc.instructor_id = di.Id
						ORDER BY dc.number_users DESC
						LIMIT 5;
					";

					result = conn.Query<CoursesModel?>(sql).ToList();
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}
	}
}