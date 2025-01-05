namespace Ballastlane.Contacts.Domain.Repository;

public interface IContactsRepository
{
    Task<Contact> CreateContact(Contact contact, CancellationToken cancellationToken);
    Task<Contact> GetContact(int ownerId, string id, CancellationToken cancellationToken);
    Task<IEnumerable<Contact>> GetContacts(int ownerId, CancellationToken cancellationToken);
    Task<Contact> UpdateContact(Contact contact, CancellationToken cancellationToken);
    Task<bool> Delete(int ownerId, string contactId, CancellationToken cancellationToken);
}
