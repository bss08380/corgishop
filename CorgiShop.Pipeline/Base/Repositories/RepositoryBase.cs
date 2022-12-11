﻿using CorgiShop.Common.Exceptions;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Model.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CorgiShop.Pipeline.Base.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class, IRepositoryEntity
{
    private readonly DbContext _context;

    protected int MaxPageSizeLimit { get; init; } = 200;

    public RepositoryBase(DbContext context)
    {
        _context = context;
    }

    /*
     * QUERY
     */

    public virtual async Task<int> Count()
    {
        return await _context.Set<T>().CountAsync();
    }

    public virtual async Task<T> GetById(int id)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null) throw DetailedException.FromFailedVerification($"{typeof(T).Name} ID", "ID could not be found");
        return entity;
    }

    public virtual async Task<IEnumerable<T>> List()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> ListPaginated(int limit, int offset)
    {
        return await _context.Set<T>()
            .Where(p => p.IsDeleted == false)
            .OrderBy(p => p.Id)
            .Skip(offset)
            .Take(Math.Min(limit, MaxPageSizeLimit))//simple db protection - enforced and returned error limits are in place further up
            .ToListAsync();
    }

    /*
     * COMMAND
     */

    public async Task<T> Create(T newEntity)
    {
        await _context.Set<T>().AddAsync(newEntity);
        await _context.SaveChangesAsync();
        return newEntity;
    }

    public async Task<T> Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(int id)
    {
        var entity = await GetById(id);
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDelete(int id)
    {
        var entity = await GetById(id);
        entity.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}
