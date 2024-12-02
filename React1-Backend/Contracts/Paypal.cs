using System.Collections.Generic;

namespace React1_Backend.Contracts;

public class GetTokenResponse
{
    public string Scope { get; set; }
    public string Access_token { get; set; }
    public string Token_type { get; set; }
    public string App_id { get; set; }
    public int Expires_in { get; set; }
    public string Nonce { get; set; }
}

public class GetTransactionsResponse
{
    public List<TransactionDetail> Transaction_details { get; set; }
    public string Account_number { get; set; }
    public string Start_date { get; set; }
    public string End_date { get; set; }
    public string Last_refreshed_datetime { get; set; }
    public int Page { get; set; }
    public int Total_items { get; set; }
    public int Total_pages { get; set; }
}

public class TransactionDetail
{
    public TransactionInfo Transaction_info { get; set; }
}

public class TransactionInfo
{
    public string Paypal_account_id { get; set; }
    public string Transaction_id { get; set; }
    public string Transaction_event_code { get; set; }
    public string Transaction_initiation_date { get; set; }
    public string Transaction_updated_date { get; set; }
    public Amount Transaction_amount { get; set; }
    public Amount Fee_amount { get; set; }
    public string Transaction_status { get; set; }
    public string Transaction_note { get; set; }
    public Amount Ending_balance { get; set; }
    public Amount Available_balance { get; set; }
    public string Protection_eligibility { get; set; }
}

public class Amount
{
    public string Currency_code { get; set; }
    public string Value { get; set; }
}

public class GetBalanceResponse
{
    public List<Balance> Balances { get; set; }
    public string Account_id { get; set; }
    public string As_of_time { get; set; }
    public string Last_refresh_time { get; set; }
}

public class Balance
{
    public string Currency { get; set; }
    public bool? Primary { get; set; }
    public Amount Total_balance { get; set; }
    public Amount Available_balance { get; set; }
    public Amount Withheld_balance { get; set; }
}
