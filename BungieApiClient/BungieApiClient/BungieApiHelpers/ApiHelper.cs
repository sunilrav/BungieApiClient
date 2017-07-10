using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BungieApiClient.BungieApiHelpers
{
    public class ApiHelper
    {
		public static async Task<string> GetBungiePlayerId(string playerName)
		{
			var playerId = await GetPlayerId(playerName, "2");
			if (playerId == "0")
			{
				playerId = await GetPlayerId(playerName, "1");
			}

			return playerId;
		}

	    private static async Task<string> GetPlayerId(string playerName, string platform)
	    {
		    var client = new HttpClient();
		    client.DefaultRequestHeaders.Clear();
		    client.DefaultRequestHeaders.Add("X-API-KEY", "de7e5188e3af4dd3b5bf481bd7f8f198");
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
	}
}
