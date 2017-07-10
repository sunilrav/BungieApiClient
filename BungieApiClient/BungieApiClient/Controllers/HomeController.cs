using System.Threading.Tasks;
using BungieApiClient.BungieApiHelpers;
using Microsoft.AspNetCore.Mvc;

namespace BungieApiClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

	    [HttpPost]
	    public async Task<IActionResult> Index(string playername, string playerId)
	    {
			if(string.IsNullOrEmpty(playerId))
				playerId = await ApiHelper.GetBungiePlayerId(playername);

		    if (playerId == "0")
		    {
			    ModelState.AddModelError("PlayerName", "Sorry, could not find player.");
			    return View();
		    }

		    return RedirectToAction("Hud", "Home", new { playerId = playerId });
	    }

	    public IActionResult Hud(string playerId)
	    {
		    return View();
	    }

	}
}
