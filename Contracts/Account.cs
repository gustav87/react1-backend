using MongoDB.Bson.Serialization.Attributes;

namespace react1_backend.Account;

public class Account
{
    [BsonId]
    public string Id => Username;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Admin { get; set; } = true;
}
