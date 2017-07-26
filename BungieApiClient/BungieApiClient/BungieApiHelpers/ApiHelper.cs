using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BungieApiClient.BungieApiHelpers
{
    public class ApiHelper
    {
	    private readonly Settings _settings;
		public ApiHelper(Settings settings)
	    {
			_settings = settings;
		}

		public async Task<string> GetBungiePlayerId(string playerName)
		{
			var playerId = await GetPlayerId(playerName, "2");
			if (playerId == "0")
			{
				playerId = await GetPlayerId(playerName, "1");
			}

			return playerId;
		}

	    private async Task<string> GetPlayerId(string playerName, string platform)
	    {
		    var client = new HttpClient();
		    client.DefaultRequestHeaders.Clear();
		    client.DefaultRequestHeaders.Add("X-API-KEY", _settings.XApiKey);
		    var response = await client.GetAsync($"http://www.bungie.net/Platform/Destiny/SearchDestinyPlayer/{platform}/{playerName}/");
			var json = await response.Content.ReadAsStringAsync();
		    var searchDestinyPlayerResults = JsonConvert.DeserializeObject<SearchDestinyPlayerResults>(json);

		    var playerId = "0";
			if (searchDestinyPlayerResults.Response.Count > 0)
		    {
			    playerId = searchDestinyPlayerResults.Response[0].membershipId;
		    }

		    return playerId;
	    }

	    public async Task<string> GetAccessToken(string authCode)
	    {
			var client = new HttpClient();
		    client.DefaultRequestHeaders.Clear();
		    var content = new StringContent($"client_id={_settings.ClientId}&grant_type=authorization_code&code={authCode}", Encoding.UTF8, "application/x-www-form-urlencoded");
		    var response = await client.PostAsync($"https://www.bungie.net/Platform/App/OAuth/Token/", content);
		    var json = await response.Content.ReadAsStringAsync();
		    var accessTokenModel = JsonConvert.DeserializeObject<AccessTokenModel>(json);

		    return accessTokenModel.Access_Token;
	    }


		public class SearchDestinyPlayerResults
	    {
		    public List<Response> Response { get; set; }
		    public int ErrorCode { get; set; }
		    public int ThrottleSeconds { get; set; }
		    public string ErrorStatus { get; set; }
		    public string Message { get; set; }
	    }

	    public class Response
	    {
		    public string iconPath { get; set; }
		    public int membershipType { get; set; }
		    public string membershipId { get; set; }
		    public string displayName { get; set; }
	    }

		public class AccessTokenModel
		{
			public string Access_Token { get; set; }
		}
	}
}
