using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace React1_Backend.Contact;

public class ContactService
{
    private readonly string mailgunDomain = Environment.GetEnvironmentVariable("mailgunDomain") ?? "";
    private readonly string mailgunApiKey = Environment.GetEnvironmentVariable("mailgunApiKey") ?? "";
    private readonly HttpClient _httpClient;
    private readonly string _mailgunUrl;

    public ContactService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        byte[] authorizationBytes = Encoding.UTF8.GetBytes($"api:{mailgunApiKey}");
        string authorizationBase64 = Convert.ToBase64String(authorizationBytes);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationBase64);
        _mailgunUrl = $"https://api.mailgun.net/v3/{mailgunDomain}/messages";
    }

    public async Task SendMail(ContactData contactData)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _mailgunUrl);

        string domain = $"domain={mailgunDomain}";
        string from = $"from=React1 Website <{contactData.Email}>";
        string to = "to=gustav87and@gmail.com";
        string subject = "subject=Message from react1 website";
        string msg = $"text=Name: {contactData.Name} \nEmail: {contactData.Email} \nMessage: {contactData.Message}";
        string body = $"{domain}&{from}&{to}&{subject}&{msg}";

        var httpcontent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
        request.Content = httpcontent;

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content, null, response.StatusCode);
        }
    }
}
