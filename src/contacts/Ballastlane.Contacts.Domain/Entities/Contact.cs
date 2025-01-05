using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ballastlane.Contacts.Domain;

public class Contact
{
    public Contact() { }

    public Contact(int ownerId,
                   string firstName,
                   string? lastName = default,
                   string? email = default,
                   string? phoneNumber = default,
                   string? address = default,
                   string? notes = default)
    {
        Id = Guid.NewGuid().ToString();
        OwnerId = ownerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Notes = notes;
    }

    [BsonId]
    public string Id { get; set; }

    [BsonRequired]
    public int OwnerId { get; set; }

    [BsonRequired]
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
}
