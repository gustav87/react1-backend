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
    private readonly string _domain;
    private readonly string _to = "to=gustav87and@gmail.com";
    private readonly string _subject = "subject=Message from react1 website";

    public ContactService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        byte[] authorizationBytes = Encoding.UTF8.GetBytes($"api:{mailgunApiKey}");
        string authorizationBase64 = Convert.ToBase64String(authorizationBytes);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationBase64);
        _mailgunUrl = $"https://api.mailgun.net/v3/{mailgunDomain}/messages";
        _domain = $"domain={mailgunDomain}";
    }

    public async Task SendMail(ContactData contactData)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _mailgunUrl);

        string from = $"from=React1 Website <{contactData.Email}>";
        string msg = $"text=Name: {contactData.Name} \nEmail: {contactData.Email} \nMessage: {contactData.Message}";
        string body = $"{_domain}&{from}&{_to}&{_subject}&{msg}";

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
