using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class Courses : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details(string id)
		{
			return View("Details");
		}
	}
}
