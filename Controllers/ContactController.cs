using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
