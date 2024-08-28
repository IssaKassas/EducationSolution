using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class Blog : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Post()
		{
			return View("Post");
		}
	}
}
