using MongoDB.Bson.Serialization.Attributes;

namespace React1_Backend.Account;

public class Account
{
    [BsonId]
    public string Id => Username;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Admin { get; set; } = false;
}
