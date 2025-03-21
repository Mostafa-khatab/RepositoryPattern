using RepositoryPattern.Core.Models;
using RepositoryPattern.Core.Repositories;
using RepositoryPattern.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public readonly AppDbContext _context;
        public IGenericRepo<Author> Authors { get; private set; }
        public IGenericRepo<Book> Books { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Authors = new GenericRepo<Author>(_context);
            Books = new GenericRepo<Book>(_context);
        }
    

        public int Compelete() => _context.SaveChanges();
        

        public void Dispose() => _context.Dispose();
        
    }
}
