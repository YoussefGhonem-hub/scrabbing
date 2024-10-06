using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace SocialMediaScraper
{
	class Program
	{
		static async Task Main(string[] args)
		{

			string platform = "linkedin";
			string identifier = "yaman-sawan";

			//string platform = "youtube";
			//string identifier = "scripter.efendi";

			//string platform = "instagram";
			//string identifier = "youssefghonem__";

			//string platform = "tiktok";
			//string identifier = "youssef.ghonem";

			var scraper = new SocialMediaScraper();

			switch (platform)
			{
				case "instagram":
					await scraper.GetInstagramFollowers(identifier);
					break;
				case "youtube":
					await scraper.GetYouTubeChannelIdFromHandle(identifier);
					break;
				case "tiktok":
					await scraper.GetTikTokFollowers(identifier);
					break;
				case "facebook":
					await scraper.GetFacebookFollowers(identifier);
					break;
				case "linkedin":
					await scraper.GetLinkedInFollowers(identifier);
					break;
				default:
					Console.WriteLine("Platform not supported. Use 'instagram', 'youtube', 'tiktok', 'facebook', or 'linkedin'.");
					break;
			}
		}
	}

	public class SocialMediaScraper
	{
		private static readonly HttpClient client = new HttpClient();

		public async Task GetInstagramFollowers(string username)
		{
			string apiKey = "NTIAYR24WWX11IY7793ASWZYCUW95L1UBQAC9P8UV0H82B1Y2LS84K5JLPJZOID7T4Z8QK0Z1PFCKJND"; // Replace with your actual ScrapingBee API key
			string url = $"https://www.instagram.com/{username}/";
			string apiUrl = $"https://app.scrapingbee.com/api/v1?api_key={apiKey}&url={url}&render_js=false";

			try
			{
				// Send request to ScrapingBee
				var response = await client.GetStringAsync(apiUrl);

				// Parse the HTML to extract the follower count
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(response);

				var metaTag = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']");
				if (metaTag != null)
				{
					var content = metaTag.GetAttributeValue("content", "");
					var followerCount = ExtractInstagramFollowerCountFromContent(content);
					Console.WriteLine($"Instagram followers for {username}: {followerCount}");
				}
				else
				{
					Console.WriteLine("Could not find follower count.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
		private string ExtractInstagramFollowerCountFromContent(string content)
		{
			// Example: "17.5m Followers, 33 Following, 1,504 Posts"
			var parts = content.Split(',');
			if (parts.Length > 0)
			{
				var followersPart = parts[0].Trim();
				return followersPart.Replace("Followers", "").Trim();
			}
			return "Unknown";
		}
		public async Task GetYouTubeChannelIdFromHandle(string channelHandle)
		{
			string apiKey = "NTIAYR24WWX11IY7793ASWZYCUW95L1UBQAC9P8UV0H82B1Y2LS84K5JLPJZOID7T4Z8QK0Z1PFCKJND"; // Replace with your actual ScrapingBee API key
			string url = $"https://www.youtube.com/@{channelHandle}";
			string apiUrl = $"https://app.scrapingbee.com/api/v1?api_key={apiKey}&url={url}&render_js=true";

			try
			{
				// Send request to ScrapingBee
				var response = await client.GetStringAsync(apiUrl);

				// Use regular expression to find the subscriber count in the response
				var match = Regex.Match(response, @"(\d{1,3}(?:[.,]\d{1,3})?[MK]?) subscribers", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					var subscriberCount = match.Groups[1].Value;
					Console.WriteLine($"YouTube subscribers for {channelHandle}: {subscriberCount}");
				}
				else
				{
					Console.WriteLine("Could not find subscriber count.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
		public async Task GetTikTokFollowers(string username)
		{
			string apiKey = "NTIAYR24WWX11IY7793ASWZYCUW95L1UBQAC9P8UV0H82B1Y2LS84K5JLPJZOID7T4Z8QK0Z1PFCKJND"; // Replace with your actual ScrapingBee API key
			string url = $"https://www.tiktok.com/@{username}";
			string apiUrl = $"https://app.scrapingbee.com/api/v1?api_key={apiKey}&url={url}&render_js=true";

			try
			{
				// Send request to ScrapingBee
				var response = await client.GetStringAsync(apiUrl);

				// Use regular expression to find the follower count in the response
				var match = Regex.Match(response, @"\""(followerCount|followers_count)\"":(\d+)", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					var followerCount = match.Groups[2].Value;
					Console.WriteLine($"TikTok followers for {username}: {followerCount}");
				}
				else
				{
					Console.WriteLine("Could not find follower count.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
		public async Task GetLinkedInFollowers(string username)
		{
			string apiKey = "NTIAYR24WWX11IY7793ASWZYCUW95L1UBQAC9P8UV0H82B1Y2LS84K5JLPJZOID7T4Z8QK0Z1PFCKJND";
			string url = $"https://www.linkedin.com/in/{username}";
			string apiUrl = $"https://app.scrapingbee.com/api/v1?api_key={apiKey}&url={url}&render_js=true";

			try
			{
				// Send request to ScrapingBee
				var response = await client.GetStringAsync(apiUrl);

				// Use regular expression to find the follower count in the response
				var match = Regex.Match(response, @"""userInteractionCount"":(\d+)", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					var followerCount = match.Groups[1].Value;
					Console.WriteLine($"LinkedIn followers for {username}: {followerCount}");
				}
				else
				{
					Console.WriteLine("Could not find follower count.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
		public async Task GetFacebookFollowers(string username)
		{
			string apiKey = "NTIAYR24WWX11IY7793ASWZYCUW95L1UBQAC9P8UV0H82B1Y2LS84K5JLPJZOID7T4Z8QK0Z1PFCKJND"; // Replace with your actual ScrapingBee API key
			string url = $"https://www.facebook.com/profile.php?id=61550338830614";
			string apiUrl = $"https://app.scrapingbee.com/api/v1?api_key={apiKey}&url={url}&render_js=true";

			try
			{
				// Send request to ScrapingBee
				var response = await client.GetStringAsync(apiUrl);

				// Use regular expression to find the follower count in the response
				var match = Regex.Match(response, @"(\d[,.\d]*) people follow this", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					var followerCount = match.Groups[1].Value;
					Console.WriteLine($"Facebook followers for {username}: {followerCount}");
				}
				else
				{
					Console.WriteLine("Could not find follower count.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
	}
}
