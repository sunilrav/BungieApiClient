using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BungieApiClient.BungieApiHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace BungieApiClient.Controllers
{
    public class HomeController : Controller
    {
	    private readonly Settings _settings;

		public HomeController(IOptions<Settings> settings)
		{
			_settings = settings.Value;
		}
        public IActionResult Index()
        {
	        var authUrl = _settings.AuthUrl;
	        var clientId = _settings.ClientId;
	        var fullAuthUrl = $"{authUrl}?client_id={clientId}&response_type=code";

	        ViewBag.FullAuthUrl = fullAuthUrl;
            return View();
        }

	    [HttpPost]
	    public async Task<IActionResult> Index(string playername, string playerId)
	    {
			if(string.IsNullOrEmpty(playerId))
				playerId = await new ApiHelper(_settings).GetBungiePlayerId(playername);

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


	    public async Task<IActionResult> Login(string code)
	    {
			var accessToken = await new ApiHelper(_settings).GetAccessToken(code);

		    var options = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
		    Response.Cookies.Append("access_token", accessToken, options);
			
			return RedirectToAction("LoginHud", "Home");
		}

	    public IActionResult LoginHud(string playerId)
	    {
		    var accessToken = Request.Cookies["access_token"];
		    return View();
	    }

	}
}
