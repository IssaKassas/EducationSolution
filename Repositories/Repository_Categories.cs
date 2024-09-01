using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class RepositoryCategories
	{
		[Table("dtbl_categories")]
		public class Model
		{
			public int Id { get; set; }

			public string? category_name { get; set; }

			public int number_courses { get; set; }
		}

		public static List<CategoriesModel?>? GetCategories()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<CategoriesModel?>? result = null;

			var sql = @"
				SELECT 
				  dca.Id AS category_id,
				  dca.category_name,
				  dca.number_courses,
				  GROUP_CONCAT(dco.course_name SEPARATOR ', ') AS courses
				FROM dtbl_categories AS dca
				INNER JOIN dtbl_courses AS dco ON dco.category_id = dca.Id
				GROUP BY dca.Id, dca.category_name;
			";

			try
			{
				using (IDbConnection conn = new MySqlConnection(Program.ConnectionString))
				{
					result = conn.Query<CategoriesModel?>(sql, new { }).ToList();
				}
			}

			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}
	}
}
