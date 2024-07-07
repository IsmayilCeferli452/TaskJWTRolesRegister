using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BookAuthorRepository : BaseRepository<BookAuthors>, IBookAuthorRepository
    {
        public BookAuthorRepository(AppDbContext context) : base(context) { }
    }
}
