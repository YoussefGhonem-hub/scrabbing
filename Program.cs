using HtmlAgilityPack; // You will need to install HtmlAgilityPack via NuGet
using System.Net;
using System.Text;
using System.Text.Json;

HttpClientHandler handler = new HttpClientHandler()
{
	AutomaticDecompression = DecompressionMethods.All
};
HttpClient client = new HttpClient(handler);

var apiKey = "5b7ef2af441d4325a2b9ebf250af593d";
var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(apiKey + ":");
var auth = System.Convert.ToBase64String(bytes);
client.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

client.DefaultRequestHeaders.Add("Accept-Encoding", "br, gzip, deflate");

var input = new Dictionary<string, object>()
{
	{"url", "https://www.linkedin.com/in/amr-elzanaty-6157a3209"},
	{"httpResponseBody", true}
};
var inputJson = JsonSerializer.Serialize(input);
var content = new StringContent(inputJson, Encoding.UTF8, "application/json");

HttpResponseMessage response = await client.PostAsync("https://api.zyte.com/v1/extract", content);
var body = await response.Content.ReadAsByteArrayAsync();

var data = JsonDocument.Parse(body);
var base64HttpResponseBody = data.RootElement.GetProperty("httpResponseBody").GetString();
var httpResponseBody = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64HttpResponseBody));

// Load the HTML content into HtmlAgilityPack
var htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml(httpResponseBody);

// Use XPath or CSS selectors to find the element that contains the number of followers
var followerNode = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(@class, 'pv-top-card--list-bullet')]/span[contains(text(), 'followers')]");

if (followerNode != null)
{
	// Extract the number of followers
	var followersText = followerNode.InnerText;
	var followers = followersText.Split(' ')[0]; // Assuming the text is something like "500 followers"
	Console.WriteLine($"Number of followers: {followers}");
}
else
{
	Console.WriteLine("Followers information not found.");
}
