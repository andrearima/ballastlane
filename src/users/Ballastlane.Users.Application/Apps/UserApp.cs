using Ballastlane.Notification;
using Ballastlane.Users.Application.Models;
using Ballastlane.Users.Domain.Entities;
using Ballastlane.Users.Domain.Repositories;

namespace Ballastlane.Users.Application.Apps;

public class UserApp : IUserApp
{
    private readonly IUserRepository _userRepository;
    private readonly INotifications _notifications;
    public UserApp(IUserRepository userRepository, INotifications notifications)
    {
        _userRepository = userRepository;
        _notifications = notifications;
    }

    public async Task<IEnumerable<UserResponse>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(cancellationToken);
        if (users is null || !users.Any())
            return [];

        return users.Select(x => new UserResponse(x));
    }

    public async Task<UserResponse?> GetUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(userId, cancellationToken);

        return user is not null ? new UserResponse(user) : null;
    }

    public async Task<UserResponse?> CreateUser(UpsertUser createUser, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(createUser.Email) || !createUser.Email.Contains('@'))
            _notifications.AddNotification("Invalid Email.");

        if (string.IsNullOrWhiteSpace(createUser.Name))
            _notifications.AddNotification("Invalid Name.");

        if (string.IsNullOrWhiteSpace(createUser.Password))
            _notifications.AddNotification("Invalid Password.");

        if (!_notifications.IsValid)
            return null;

        createUser.Email = createUser.Email!.ToLower();
        var userExists = await _userRepository.GetUser(createUser.Email, cancellationToken);
        if (userExists is not null)
        {
            _notifications.AddNotification("User with email already register.");
            return null;
        }

        var userDomain = await _userRepository.CreateUser(new User(createUser.Name!, createUser.Email, createUser.Password!), cancellationToken);
        if (userDomain is null)
        {
            _notifications.AddNotification("Fail to register the user on repository.");
            return null;
        }

        return new UserResponse(userDomain);
    }

    public async Task<UserResponse?> UpdateUser(int userId, UpsertUser updateUser, CancellationToken cancellationToken)
    {
        var userDb = await _userRepository.GetUser(userId, cancellationToken);
        if (userDb is null)
            return null;

        if (string.IsNullOrWhiteSpace(updateUser.Email) || !updateUser.Email.Contains('@'))
            _notifications.AddNotification("Invalid Email.");

        if (string.IsNullOrWhiteSpace(updateUser.Name))
            _notifications.AddNotification("Invalid Name.");

        if (string.IsNullOrWhiteSpace(updateUser.Password))
            _notifications.AddNotification("Invalid Password.");

        if (!_notifications.IsValid)
            return null;

        updateUser.Email = updateUser.Email!.ToLower();
        var userExists = await _userRepository.GetUser(updateUser.Email, cancellationToken);
        if (userExists is not null && userExists.Id != userDb.Id)
        {
            _notifications.AddNotification("User with email already register.");
            return null;
        }

        var userDomain = await _userRepository.UpdateUser(new User(updateUser.Name!, updateUser.Email, updateUser.Password!), cancellationToken);
        if (userDomain is null)
        {
            _notifications.AddNotification("Fail to register the user on repository.");
            return null;
        }

        return new UserResponse(userDomain);
    }

    public async Task<bool> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(userId, cancellationToken);
        if (user is null)
            return false;

        return await _userRepository.DeleteUser(user, cancellationToken);
    }
}
