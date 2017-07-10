using System.ComponentModel.DataAnnotations;

namespace BungieApiClient.ViewModels
{
    public class PlayerInfo
    {
	    [Required]
	    public string PlayerName { get; set; }
	}
}
