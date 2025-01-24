namespace Bookify.Domain.Users;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid userId,CancellationToken cancellationToken);
    void AddUser(User user );
}