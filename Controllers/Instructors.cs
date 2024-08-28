using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class Instructors : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult CoursesByInstructor(string id)
		{
			return View("CoursesByInstructor");
		}
	}
}
