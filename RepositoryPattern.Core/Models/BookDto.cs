using RepositoryPattern.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPattern.Api.Controllers
{
    public class BookDto
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public int AuthorId { get; set; }
    }
}
