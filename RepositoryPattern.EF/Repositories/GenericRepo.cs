using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Core.Repositories;
using RepositoryPattern.EF.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryPattern.EF.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Cannot create a null entity.");
            await _context.Set<T>().AddAsync(item);
            return item;
        }

        public T Delete(int id)
        {
            var res = _context.Set<T>().Find(id);
            if (res == null)
                throw new ArgumentException("The entity with the given Id was not found.");
            _context.Set<T>().Remove(res);
            return res;
        }

        public async Task<T> GetAsync(int id)
        {
            var res = await _context.Set<T>().FindAsync(id);
            if (res == null)
                throw new ArgumentException("Entity not found.");
            return res;
        }

        public T Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Cannot update a null entity.");
            _context.Set<T>().Update(item);
            return item;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
