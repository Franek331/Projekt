using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SeeSharp.Egzaminer.Domain.Entities;
using System.Linq.Expressions;
using SeeSharp.Egzaminer.Application.Interfaces;

namespace SeeSharp.Egzaminer.Infrastructure.Repositories
{
    public class KeyRepository<T, TKey> : BaseRepository<T>, IKeyRepository<T, TKey>
        where T : BaseEntity<TKey>
        where TKey : struct
    {
        private readonly DbContext _context;

        public KeyRepository(DbContext context, ILogger<IRepository<T>> logger)
            : base(context, logger)
        {
            _context = context;
        }

        // Implementacja metody GetPendingGradingCount
        public async Task<int> GetPendingGradingCount()
        {
            var pendingCount = await _context.Set<TestPublication>()
                .Where(t => t.AnswerSubmitted.Any(a => !a.IsManuallyGraded)) // Odpowiedzi, które nie zostały ocenione
                .CountAsync();

            return pendingCount;
        }

        public async Task<T?> GetById(TKey id)
            => await DbSet.FindAsync(id);

        public async Task<T?> GetById<TProperty>(TKey id, Expression<Func<T, TProperty>> include)
            => await DbSet.Include(include).FirstOrDefaultAsync(x => x.Id.Equals(id));

        public async Task DeleteAsync(TKey id)
        {
            T? entity = await DbSet.FindAsync(id);
            if (entity == null)
                throw new InvalidOperationException($"Entity {typeof(T).Name} with id {id} not found.");

            DbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
