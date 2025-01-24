using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Users;

public sealed class User :Entity
{
    public User(Guid id,FirstName firstName,LastName lastname,Email email) : base(id)
    {
        Firstname = firstName;
        Lastname = lastname;
        this.Email = email;

    }

    public FirstName Firstname { get; private set; }
    public LastName Lastname { get; private set; }
    public Email Email { get; private set; }

    public static User Create(FirstName firstName,LastName lastname,Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastname, email);
        user.RaiseDomainEvents(new UserCreatedDomainevents(user.Id));
        return user;
    }
}