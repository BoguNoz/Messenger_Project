using Models;

namespace Repositories;

public abstract class RavenBaseRepository
{
    protected readonly DbContext dbContext;

    protected RavenBaseRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }
}