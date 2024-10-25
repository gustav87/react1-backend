namespace React1_backend.Contracts;

public class GetTokenResponse
{
  public string Scope { get; set; } = string.Empty;
  public string Access_token { get; set; } = string.Empty;
  public string Token_type { get; set; } = string.Empty;
  public string App_id { get; set; } = string.Empty;
  public int Expires_in { get; set; }
  public string Nonce { get; set; } = string.Empty;
}

public class GetTransactionsResponse
{
  public List<TransactionDetail> Transaction_details { get; set; } = null!;
  public string Account_number { get; set; } = string.Empty;
  public string Start_date { get; set; } = string.Empty;
  public string End_date { get; set; } = string.Empty;
  public string Last_refreshed_datetime { get; set; } = string.Empty;
  public int Page { get; set; }
  public int Total_items { get; set; }
  public int Total_pages { get; set; }
}

public class TransactionDetail
{
  public TransactionInfo Transaction_info { get; set; } = null!;
}

public class TransactionInfo
{
  public string Paypal_account_id { get; set; } = string.Empty;
  public string Transaction_id { get; set; } = string.Empty;
  public string Transaction_event_code { get; set; } = string.Empty;
  public string Transaction_initiation_date { get; set; } = string.Empty;
  public string Transaction_updated_date { get; set; } = string.Empty;
  public Amount Transaction_amount { get; set; } = null!;
  public Amount? Fee_amount { get; set; }
  public string Transaction_status { get; set; } = string.Empty;
  public string Transaction_note { get; set; } = string.Empty;
  public Amount Ending_balance { get; set; } = null!;
  public Amount Available_balance { get; set; } = null!;
  public string Protection_eligibility { get; set; } = string.Empty;
}

public class Amount
{
  public string Currency_code { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
}

public class GetBalanceResponse
{
  public List<Balance> Balances { get; set; } = null!;
  public string Account_id { get; set; } = string.Empty;
  public string As_of_time { get; set; } = string.Empty;
  public string Last_refresh_time { get; set; } = string.Empty;
}

public class Balance
{
  public string Currency { get; set; } = string.Empty;
  public bool? Primary { get; set; }
  public Amount Total_balance { get; set; } = null!;
  public Amount Available_balance { get; set; } = null!;
  public Amount Withheld_balance { get; set; } = null!;
}
