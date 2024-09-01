using Dapper;
using MySql.Data.MySqlClient;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class RepositoryStudents
    {
		public class Model
		{
			public int Id { get; set; }

			public string? student_name { get; set; }
		}

		public static List<TestimonialsModel?>? GetTestimonials()
		{
			SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
			List<TestimonialsModel?>? result = null;

			try
			{
				using (var conn = new MySqlConnection(Program.ConnectionString))
				{
					var sql = @"
						SELECT * 
						FROM dtbl_testimonials AS dt
						INNER JOIN dtbl_instructors AS di 
						ON dt.instructor_id = di.Id
						INNER JOIN dtbl_students AS ds 
						ON dt.student_id = ds.Id
						ORDER BY di.rate DESC
						LIMIT 3;
					";

					result = conn.Query<TestimonialsModel?>(sql).ToList();
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.Message); }

			return result;
		}
	}
}
