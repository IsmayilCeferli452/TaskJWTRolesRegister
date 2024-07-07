using Service.DTOs.Admin.BookAuthors;
using Service.DTOs.Admin.Books;

namespace Service.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto> GetByIdAsync(int? id);
        Task CreateAsync(BookCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, BookEditDto model);
        Task AddGroupAsync(BookAuthorCreateDto model);
    }
}
