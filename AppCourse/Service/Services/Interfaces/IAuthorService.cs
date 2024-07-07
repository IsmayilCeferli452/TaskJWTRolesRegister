using Service.DTOs.Admin.Authors;

namespace Service.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync();
        Task<AuthorDto> GetByIdAsync(int? id);
        Task CreateAsync(AuthorCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, AuthorEditDto model);
    }
}
