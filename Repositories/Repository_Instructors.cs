using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class RepositoryInstructors
	{
		[Table("dtbl_instructors")]
		public class Model
		{
			[Key]
			public int Id { get; set; }

			public string? instructor_name { get; set; }

			public double rate { get; set; }

			public int available_courses { get; set; }

			public string? instructor_image { get; set; }

			public string? cover_letter { get; set; }
		}

		public static List<Model?>? GetInstrucctors()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<Model?>? result = null;

			var sql = "SELECT * FROM dtbl_instructors";

			try
			{
				using (IDbConnection conn = new MySqlConnection(Program.ConnectionString))
				{
					result = conn.Query<Model?>(sql, new {}).ToList();
				}
			}

			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}

		public static List<BlogsModel?>? GetBlogs()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<BlogsModel?>? result = null;

			var sql = @"
				SELECT *, db.Id AS blog_id 
				FROM dtbl_instructors AS di
				INNER JOIN dtbl_blogs AS db
				ON db.instructor_id = di.Id
				ORDER BY db.dob DESC LIMIT 3;
			";

			try
			{
				using (IDbConnection conn = new MySqlConnection(Program.ConnectionString))
				{
					result = conn.Query<BlogsModel?>(sql, new { }).ToList();
				}
			}

			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}
	}
}
