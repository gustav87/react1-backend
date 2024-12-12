using MongoDB.Driver;
using System.Threading.Tasks;

namespace React1_Backend.Contact;

public class ContactRepository
{
    private readonly IMongoCollection<Mail> _mailCollection;

    public ContactRepository(MongoClient mongoClient)
    {
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase("React1-Backend");
        _mailCollection = mongoDatabase.GetCollection<Mail>("Mail");
    }

    public async Task Insert(Mail mail) => await _mailCollection.InsertOneAsync(mail);
}
