using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository:Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}