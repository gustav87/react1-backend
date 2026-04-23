using MongoDB.Driver;
using System.Threading.Tasks;

namespace Citrus_Backend.Contact;

public class ContactRepository
{
    private readonly IMongoCollection<Mail> _mailCollection;

    public ContactRepository(MongoClient mongoClient)
    {
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase("citrus-db");
        _mailCollection = mongoDatabase.GetCollection<Mail>(nameof(Mail));
    }

    public async Task Insert(Mail mail) => await _mailCollection.InsertOneAsync(mail);
}
