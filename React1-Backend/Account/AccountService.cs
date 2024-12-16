using React1_Backend.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace React1_Backend.Account;

public class AccountService(AccountRepository accountRepository) : IAccountService
{
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly string adminToken = Environment.GetEnvironmentVariable("adminToken") ?? "";
    private readonly string userToken = Environment.GetEnvironmentVariable("userToken") ?? "";

    public async Task<string> LogIn(Account req)
    {
        Console.WriteLine($"Log in attempt from {req.Username}.");
        Account res = await _accountRepository.GetAsyncByUsername(req.Username);
        if (res != null && SecretHasher.Verify(req.Password, res.Password))
        {
            return res.Admin ? adminToken : userToken;
        }
        return "";
    }

    public async Task CreateAccount(SignUpData req)
    {
        Account hashedAccount = new() { Username = req.Username, Password = SecretHasher.Hash(req.Password) };
        await _accountRepository.CreateAsync(hashedAccount);
    }

    public async Task<List<Account>> GetAsync()
    {
        return await _accountRepository.GetAsync();
    }

    public async Task<Account> GetAsyncByUsername(string username)
    {
        return await _accountRepository.GetAsyncByUsername(username);
    }

    public bool IsEmpty(Account account)
    {
        return string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password);
    }

    // private static Account ConvertAccountToHashed(Account input)
    // {
    //         using SHA256 sha = SHA256.Create();
    //         byte[] asBytes = Encoding.UTF8.GetBytes(input.Password);
    //         string hashed = Convert.ToBase64String(sha.ComputeHash(asBytes));
    //         Account hashedAccount = new Account() { Username = input.Username, Password = hashed };
    //         return hashedAccount;
    // }
}
