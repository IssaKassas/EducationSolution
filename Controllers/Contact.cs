using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
	public class Contact : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
