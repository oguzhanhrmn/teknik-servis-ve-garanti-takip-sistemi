#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Tests.Fakes;

public class FakeGenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly List<T> _items = new();
    private int _idSeq = 1;

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<T>>(_items.ToList());
    }

    public Task<T?> GetByIdAsync(int id)
    {
        var prop = typeof(T).GetProperty("Id");
        var item = _items.FirstOrDefault(x => prop?.GetValue(x) is int v && v == id);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var result = _items.AsQueryable().Where(predicate).ToList();
        return Task.FromResult<IEnumerable<T>>(result);
    }

    public Task AddAsync(T entity)
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop != null && prop.CanWrite && (int)(prop.GetValue(entity) ?? 0) == 0)
        {
            prop.SetValue(entity, _idSeq++);
        }
        _items.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(T entity)
    {
        // no-op for in-memory list; entity reference already updated
    }

    public void Delete(T entity)
    {
        _items.Remove(entity);
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(1);
    }
}
