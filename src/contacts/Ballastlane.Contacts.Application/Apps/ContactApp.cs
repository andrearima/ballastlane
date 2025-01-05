using Ballastlane.Contacts.Application.Models;
using Ballastlane.Contacts.Domain;
using Ballastlane.Contacts.Domain.Repository;
using Ballastlane.Notification;

namespace Ballastlane.Contacts.Application.Apps;

public class ContactApp : IContactApp
{
    private readonly IContactsRepository _contactsRepository;
    private readonly INotifications _notifications;

    public ContactApp(IContactsRepository contactsRepository, INotifications notifications)
    {
        _contactsRepository = contactsRepository;
        _notifications = notifications;
    }

    public async Task<Contact> CreateContact(int userId, CreateContact createContact, CancellationToken cancellationToken)
    {
        var contact = new Contact(userId,
                                  createContact.FirstName,
                                  createContact.LastName,
                                  createContact.Email,
                                  createContact.PhoneNumber,
                                  createContact.Address,
                                  createContact.Notes);

        return await _contactsRepository.CreateContact(contact, cancellationToken);
    }

    public async Task<Contact?> GetContact(int userId, string objectId, CancellationToken cancellationToken)
    {
        return await _contactsRepository.GetContact(userId, objectId, cancellationToken);
    }

    public async Task<IEnumerable<Contact>> GetContacts(int userId, CancellationToken cancellationToken)
    {
        return await _contactsRepository.GetContacts(userId, cancellationToken);
    }

    public async Task<Contact?> UpdateContact(int userId, string objectId, UpdateContact updateContact, CancellationToken cancellationToken)
    {
        var contact = await GetContact(userId, objectId, cancellationToken);
        if (!_notifications.IsValid)
            return null;

        if (contact is null)
            return null;

        contact.FirstName = updateContact.FirstName ?? contact.FirstName;
        contact.LastName = updateContact.LastName;
        contact.Email = updateContact.Email;
        contact.PhoneNumber = updateContact.PhoneNumber;
        contact.Address = updateContact.Address;
        contact.Notes = updateContact.Notes;

        return await _contactsRepository.UpdateContact(contact, cancellationToken);
    }

    public async Task<bool> DeleteContact(int userId, string objectId, CancellationToken cancellationToken)
    {
        return await _contactsRepository.Delete(userId, objectId, cancellationToken);
    }
}
