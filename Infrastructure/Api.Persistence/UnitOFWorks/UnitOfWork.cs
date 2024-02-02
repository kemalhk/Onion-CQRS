using Api.Application.Interfaces.Repositories;
using Api.Application.Interfaces.UnitOfWorks;
using Api.Persistence.Context;
using Api.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.UnitOFWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

        public int Save() => dbContext.SaveChanges();
        public async Task<int> SaveAsync() => await dbContext.SaveChangesAsync();
        IReadRepository<T> IUnitOfWork.GetReadRepository<T>() => new ReadRepository<T>(dbContext);
        IWriteRepository<T> IUnitOfWork.GetWriteRepository<T>() => new WriteRepository<T>(dbContext);
    }
}
