﻿namespace CorgiShop.Domain.Abstractions;

public interface IQueryRepository<T> where T : IRepositoryEntity
{
    Task<int> Count();
    Task<IEnumerable<T>> List();
    Task<IEnumerable<T>> ListPaginated(int limit, int offset);
    Task<T> GetById(int id);
}