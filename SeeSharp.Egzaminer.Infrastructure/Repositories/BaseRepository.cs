using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using SeeSharp.Egzaminer.Application.Interfaces;
using SeeSharp.Egzaminer.Domain.Entities;
using SeeSharp.Egzaminer.Domain.ValueObjects;

namespace SeeSharp.Egzaminer.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly ILogger<IRepository<T>> logger;

    public BaseRepository(DbContext dbContext, ILogger<IRepository<T>> logger)
    {
        this.logger = logger;
        Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext),
            "An instance of DbContext is required to use this repository");
        DbSet = Context.Set<T>();
        Context.Database.SetCommandTimeout(TimeSpan.FromHours(1));
    }

    protected DbContext Context { get; set; }

    protected DbSet<T> DbSet { get; set; }

    public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        => DbSet.Where(predicate);

    public virtual IQueryable<T> GetAll() => DbSet;

    public virtual void Add(T entity)
    {
        EntityEntry dbEntityEntry = Context.Entry(entity);
        if (dbEntityEntry.State != EntityState.Detached)
            dbEntityEntry.State = EntityState.Added;
        else
            DbSet.Add(entity);

        SetAuditation(entity, dbEntityEntry);
        Context.SaveChanges();
    }

    public virtual async Task AddAsync(T entity)
    {
        EntityEntry dbEntityEntry = Context.Entry(entity);
        if (dbEntityEntry.State != EntityState.Detached)
            dbEntityEntry.State = EntityState.Added;
        else
            await DbSet.AddAsync(entity);

        SetAuditation(entity, dbEntityEntry);
        await Context.SaveChangesAsync();
    }

    public virtual void Update(T entity)
    {
        EntityEntry dbEntityEntry = UpdateState(entity);

        SetAuditation(entity, dbEntityEntry);
        Context.SaveChanges();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        EntityEntry dbEntityEntry = UpdateState(entity);

        SetAuditation(entity, dbEntityEntry);
        await Context.SaveChangesAsync();
    }

    private EntityEntry UpdateState(T entity)
    {
        EntityEntry dbEntityEntry = Context.Entry(entity);
        if (dbEntityEntry.State == EntityState.Detached) DbSet.Attach(entity);
        dbEntityEntry.State = EntityState.Modified;
        return dbEntityEntry;
    }

    public virtual void Delete(T entity)
    {
        EntityEntry dbEntityEntry = Context.Entry(entity);
        if (dbEntityEntry.State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }
        DbSet.Remove(entity);
        Context.SaveChanges();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        EntityEntry dbEntityEntry = Context.Entry(entity);
        if (dbEntityEntry.State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        var enumerable = entities as T[] ?? entities.ToArray();
        Context.Set<T>().AddRange(enumerable);
        foreach (T entity in enumerable) SetAuditation(entity, Context.Entry(entity));
        Context.SaveChanges();
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        var enumerable = entities as T[] ?? entities.ToArray();
        Context.Set<T>().AddRange(enumerable);
        foreach (T entity in enumerable) SetAuditation(entity, Context.Entry(entity));
        await Context.SaveChangesAsync();
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
        Context.SaveChanges();
    }

    public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
        await Context.SaveChangesAsync();
    }

    public async Task ApplyPatchAsync(T entity, params Patch[] patches)
    {
        await using var transaction = await Context.Database.BeginTransactionAsync();
        Dictionary<string, object> nameValuePairProperties = patches.ToDictionary(a => a.PropertyName, a => a.PropertyValue)!;
        EntityEntry<T> dbEntityEntry = Context.Entry(entity);
        dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
        if (entity is BaseAuditedEntity auditedEntity)
        {
            auditedEntity.ModifiedAt = DateTime.Now;
        }

        dbEntityEntry.State = EntityState.Modified;
        await Context.SaveChangesAsync();
        await transaction.CommitAsync();
        await Context.Entry(entity).ReloadAsync();
    }

    private void SetAuditation(T entity, EntityEntry dbEntityEntry)
    {
        if (entity is BaseAuditedEntity auditedEntity)
        {
            try
            {
                if (dbEntityEntry.State == EntityState.Added)
                {
                    auditedEntity.CreateAt = DateTime.Now;
                }

                auditedEntity.ModifiedAt = DateTime.Now;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}