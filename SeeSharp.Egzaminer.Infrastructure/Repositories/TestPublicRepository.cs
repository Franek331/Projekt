using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using SeeSharp.Egzaminer.Application.Interfaces;
using SeeSharp.Egzaminer.Infrastructure.Persistence;
using System;
using System.Linq.Expressions;
using SeeSharp.Egzaminer.Domain.ValueObjects;

namespace SeeSharp.Egzaminer.Infrastructure.Repositories;

public class TestPublicationRepository : ITestPublicationRepository
{
    private readonly AppDbContext _context;

    public TestPublicationRepository(AppDbContext context)
    {
        _context = context;
    }

    // Implementacja metody GetPendingGradingCount
    public async Task<int> GetPendingGradingCount()
    {
        var pendingCount = await _context.AnswerSubmitted
        .Where(a => !a.IsManuallyGraded) // Tylko odpowiedzi, które nie zostały ocenione
        .CountAsync();

        return pendingCount;
    }

    // Implementacja metody GetById<TProperty> - pobiera TestPublication oraz dołącza dodatkową właściwość (np. Test)
    public async Task<TestPublication?> GetById<TProperty>(Guid id, Expression<Func<TestPublication, TProperty>> include)
    {
        // Pobieramy TestPublication i dołączamy właściwość zdefiniowaną przez include (np. Test)
        return await _context.TestPublications
            .Include(include)  // Dołączamy wskazaną właściwość
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Implementacja metody GetById(Guid) - pobiera TestPublication po ID
    public async Task<TestPublication?> GetById(Guid id)
    {
        return await _context.TestPublications.FirstOrDefaultAsync(x => x.Id == id);
    }

    // Implementacja metody Add - dodaje TestPublication do bazy
    public void Add(TestPublication entity)
    {
        _context.TestPublications.Add(entity);
    }

    // Implementacja metody AddAsync - dodaje TestPublication asynchronicznie do bazy
    public async Task AddAsync(TestPublication entity)
    {
        await _context.TestPublications.AddAsync(entity);
    }

    // Implementacja metody DeleteAsync - usuwa TestPublication asynchronicznie z bazy
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.TestPublications.FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
        {
            _context.TestPublications.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public void AddRange(IEnumerable<TestPublication> entities)
    {
        _context.TestPublications.AddRange(entities);
        _context.SaveChanges();  // Zapisujemy zmiany w bazie danych
    }

    public async Task AddRangeAsync(IEnumerable<TestPublication> entities)
    {
        await _context.TestPublications.AddRangeAsync(entities);
        await _context.SaveChangesAsync();  // Asynchroniczny zapis w bazie danych
    }

    public void Update(TestPublication entity)
    {
        _context.TestPublications.Update(entity);  // Oznacza rekord jako zaktualizowany
        _context.SaveChanges();  // Zapisujemy zmiany w bazie
    }

    public async Task UpdateAsync(TestPublication entity)
    {
        _context.TestPublications.Update(entity);  // Oznacza rekord jako zaktualizowany
        await _context.SaveChangesAsync();  // Asynchroniczny zapis w bazie
    }

    public void Delete(TestPublication entity)
    {
        _context.TestPublications.Remove(entity);  // Usuwamy rekord
        _context.SaveChanges();  // Zapisujemy zmiany w bazie
    }

    public async Task DeleteAsync(TestPublication entity)
    {
        _context.TestPublications.Remove(entity);  // Usuwamy rekord
        await _context.SaveChangesAsync();  // Asynchroniczny zapis w bazie
    }

    public void RemoveRange(IEnumerable<TestPublication> entities)
    {
        _context.TestPublications.RemoveRange(entities);  // Usuwamy wiele rekordów
        _context.SaveChanges();  // Zapisujemy zmiany w bazie
    }

    public async Task RemoveRangeAsync(IEnumerable<TestPublication> entities)
    {
        _context.TestPublications.RemoveRange(entities);  // Usuwamy wiele rekordów
        await _context.SaveChangesAsync();  // Asynchroniczny zapis w bazie
    }

    public IQueryable<TestPublication> GetAll()
    {
        throw new NotImplementedException();
    }

    public IQueryable<TestPublication> Find(Expression<Func<TestPublication, bool>> predicate)
    {
        return _context.TestPublications.Where(predicate);
    }

    public async Task ApplyPatchAsync(TestPublication entity, params Patch[] patches)
    {
        var entry = _context.Entry(entity);
        foreach (var patch in patches)
        {
            entry.Property(patch.PropertyName).CurrentValue = patch.PropertyValue;
        }
        await _context.SaveChangesAsync();
    }

    // Inne metody repozytorium mogą być zaimplementowane w podobny sposób
}
