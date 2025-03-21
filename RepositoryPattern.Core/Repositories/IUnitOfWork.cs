using RepositoryPattern.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<Author> Authors { get; }
        IGenericRepo<Book> Books { get; }

        int Compelete();
    }
}
