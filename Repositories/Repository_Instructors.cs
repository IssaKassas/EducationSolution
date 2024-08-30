using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class RepositoryInstructors
	{
		public class Model
		{
			[Key]
			public int Id { get; set; }

			public string? name_instructor { get; set; }

			public double rate { get; set; }

			public int available_courses { get; set; }

			public string? Image { get; set; }

			public string? cover_letter { get; set; }
		}


		public static List<CoursesModel?>? GetCourses()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<CoursesModel?>? result = null;

			var sql = @"
				SELECT * FROM dtbl_courses AS dc
				INNER JOIN dtbl_instructors AS di
				ON dc.instructor_id = di.Id
			";

			try
			{
				using (IDbConnection conn = new MySqlConnection(Program.ConnectionString))
				{
					result = conn.GetList<CoursesModel?>(sql, new {}).ToList();
				}
			}

			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}
	}
}
