using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using React1_backend.Contracts;

namespace React1_backend.Paypal;

public class PaypalService
{
  private readonly string paypalClientId = Environment.GetEnvironmentVariable("paypalClientId") ?? "";
  private readonly string paypalSecretKey = Environment.GetEnvironmentVariable("paypalSecretKey") ?? "";
  private readonly string paypalApiUrl = "https://api-m.paypal.com/v1";
  private string token = "";

  public async Task<List<Amount>> GetTransactions()
  {
    DateTime now = DateTime.Now;
    string currentYear = now.Year.ToString();
    string currentMonth = now.ToString("MM");
    string nextMonth = now.AddMonths(1).ToString("MM");
    string startDate = $"{currentYear}-{currentMonth}-01T00:00:00-0000";
    string endDate = $"{currentYear}-{nextMonth}-01T00:00:00-0000";
    string url = $"{paypalApiUrl}/reporting/transactions?start_date={startDate}&end_date={endDate}&fields=all";

    HttpResponseMessage response = await SendRequestWithToken(HttpMethod.Get, url);

    if (response.StatusCode == HttpStatusCode.Unauthorized)
    {
      await GetToken();
      response = await SendRequestWithToken(HttpMethod.Get, url);
    }

    if (!response.IsSuccessStatusCode)
    {
      throw new Exception("Something went wrong");
    }

    string content = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(content)) throw new Exception("Something went wrong");
    var deserializeObject = JsonConvert.DeserializeObject<GetTransactionsResponse>(content) ?? throw new Exception("Something went wrong");

    List<Amount> amounts = deserializeObject.Transaction_details
      .Where(x => x.Transaction_info.Fee_amount is not null)
      .Select(x => x.Transaction_info.Transaction_amount)
      .ToList();

    return amounts ?? throw new Exception("");
  }

  public async Task<GetBalanceResponse> GetBalance()
  {
    string url = $"{paypalApiUrl}/reporting/balances";
    HttpResponseMessage response = await SendRequestWithToken(HttpMethod.Get, url);

    if (response.StatusCode == HttpStatusCode.Unauthorized)
    {
      await GetToken();
      response = await SendRequestWithToken(HttpMethod.Get, url);
    }

    if (!response.IsSuccessStatusCode)
    {
      throw new Exception("Something went wrong");
    }

    string content = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(content)) throw new Exception("Something went wrong");
    var deserializeObject = JsonConvert.DeserializeObject<GetBalanceResponse>(content);

    return deserializeObject ?? throw new Exception("Something went wrong");
  }

  private async Task GetToken()
  {
    string url = $"{paypalApiUrl}/oauth2/token";
    var httpClient = new HttpClient();
    var authorizationBytes = Encoding.UTF8.GetBytes($"{paypalClientId}:{paypalSecretKey}");
    string authorizationBase64 = Convert.ToBase64String(authorizationBytes);

    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationBase64);
    var request = new HttpRequestMessage(HttpMethod.Post, url);
    var httpcontent = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
    request.Content = httpcontent;

    HttpResponseMessage response = await httpClient.SendAsync(request);

    if (response.StatusCode == HttpStatusCode.Unauthorized)
    {
      throw new Exception("Paypal client ID or secret incorrect.");
    }

    if (!response.IsSuccessStatusCode)
    {
      throw new Exception("Something went wrong");
    }

    string content = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(content)) throw new Exception("Something went wrong");
    var deserializeObject = JsonConvert.DeserializeObject<GetTokenResponse>(content);

    token = deserializeObject!.Access_token ?? throw new Exception("Something went wrong");
  }

  private async Task<HttpResponseMessage> SendRequestWithToken(HttpMethod method, string url)
  {
    var httpClient = new HttpClient();
    var request = new HttpRequestMessage(method, url);
    request.Headers.Add("Authorization", $"Bearer {token}");
    return await httpClient.SendAsync(request);
  }
}
