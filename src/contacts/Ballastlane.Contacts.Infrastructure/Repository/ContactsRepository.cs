using Ballastlane.Contacts.Domain;
using Ballastlane.Contacts.Domain.Repository;
using MongoDB.Driver;

namespace Ballastlane.Contacts.Infrastructure.Repository;

public class ContactsRepository : IContactsRepository
{
    private readonly IMongoDatabase _mongoDb;

    public ContactsRepository(IMongoDatabase mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<Contact> CreateContact(Contact contact, CancellationToken cancellationToken)
    {
        var collection = _mongoDb.GetCollection<Contact>(contact.OwnerId.ToString());

        var options = new InsertOneOptions();
        await collection.InsertOneAsync(contact, options, cancellationToken);

        return contact;
    }

    public async Task<Contact> GetContact(int ownerId, string id, CancellationToken cancellationToken)
    {
        var collection = _mongoDb.GetCollection<Contact>(ownerId.ToString());

        using var asyncCursorResult = await collection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);

        return await asyncCursorResult.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Contact>> GetContacts(int ownerId, CancellationToken cancellationToken)
    {
        var collection = _mongoDb.GetCollection<Contact>(ownerId.ToString());

        var filter = Builders<Contact>.Filter.Empty;
        using var asyncCursorResult = await collection.FindAsync<Contact>(filter, cancellationToken: cancellationToken);

        return await asyncCursorResult.ToListAsync(cancellationToken);
    }

    public async Task<Contact> UpdateContact(Contact updateContact, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Contact>.Update
            .Set(contact => contact.FirstName, updateContact.FirstName)
            .Set(contact => contact.LastName, updateContact.LastName)
            .Set(contact => contact.Email, updateContact.Email)
            .Set(contact => contact.PhoneNumber, updateContact.PhoneNumber)
            .Set(contact => contact.Address, updateContact.Address)
            .Set(contact => contact.Notes, updateContact.Notes);

        var collection = _mongoDb.GetCollection<Contact>(updateContact.OwnerId.ToString());
        var result = await collection.UpdateOneAsync(
            filter => filter.Id == updateContact.Id,
            updateDefinition, cancellationToken: cancellationToken);

        return updateContact;
    }

    public async Task<bool> Delete(int ownerId, string contactId, CancellationToken cancellationToken)
    {
        var collection = _mongoDb.GetCollection<Contact>(ownerId.ToString());
        var result = await collection.DeleteOneAsync(x => x.Id == contactId, cancellationToken);
        return result.DeletedCount > 0;
    }
}
