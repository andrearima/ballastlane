using Ballastlane.Contacts.Application.Models;
using Ballastlane.Contacts.Domain;

namespace Ballastlane.Contacts.Application.Apps;

public interface IContactApp
{
    Task<Contact> CreateContact(int userId, CreateContact createContact, CancellationToken cancellationToken);
    Task<Contact?> GetContact(int userId, string objectId, CancellationToken cancellationToken);
    Task<IEnumerable<Contact>> GetContacts(int userId, CancellationToken cancellationToken);
    Task<Contact?> UpdateContact(int userId, string objectId, UpdateContact updateContact, CancellationToken cancellationToken);
    Task<bool> DeleteContact(int userId, string objectId, CancellationToken cancellationToken);
}