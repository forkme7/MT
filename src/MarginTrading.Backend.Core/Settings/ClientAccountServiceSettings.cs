using Lykke.SettingsReader.Attributes;

namespace MarginTrading.Backend.Core.Settings
{
	public class ClientAccountServiceSettings
	{
		[HttpCheck("/api/isalive")]
		public string ServiceUrl { get; set; }
	}
}