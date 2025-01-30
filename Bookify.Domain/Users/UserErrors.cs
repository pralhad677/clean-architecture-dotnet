using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users;

public class UserErrors
{
    public static Error NotFound = new(
        "Users.Found",


        "The User with the specified Identified was not found. "
    );
}