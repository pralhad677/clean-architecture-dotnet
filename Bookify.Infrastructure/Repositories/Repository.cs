using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

public abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext Context;

    protected Repository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Set<T>().FirstOrDefaultAsync(user=>user.Id==id, cancellationToken);
    }

    public void Add(T entity)
    {
        Context.Add(entity);
        // Context.Set<T>().Add(entity);
    }
}