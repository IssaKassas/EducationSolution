﻿using Dapper;
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

			public string? image_instructor { get; set; }

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
	}
}
