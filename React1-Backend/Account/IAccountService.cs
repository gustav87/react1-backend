using System.Collections.Generic;
using System.Threading.Tasks;

namespace React1_Backend.Account;

public interface IAccountService
{

    Task<string> LogIn(Account req);
    Task CreateAccount(SignUpData req);
    Task<List<Account>> GetAsync();
    Task<Account> GetAsync(string id);
    Task<Account> GetAsyncByUsername(string username);
    Task CreateAsync(Account account);
    Task UpdateAsync(string id, Account updatedAccount);
    Task RemoveAsync(string id);
    bool IsEmpty(Account account);
}
