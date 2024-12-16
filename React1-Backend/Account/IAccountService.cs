using System.Collections.Generic;
using System.Threading.Tasks;

namespace React1_Backend.Account;

public interface IAccountService
{

    Task<string> LogIn(Account req);
    Task CreateAccount(SignUpData req);
    Task<List<Account>> GetAsync();
    Task<Account> GetAsyncByUsername(string username);
    bool IsEmpty(Account account);
}
