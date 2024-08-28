using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class About : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
