using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Authors;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    [Authorize]
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IMapper _mapper;
        private readonly IBookAuthorRepository _bookAuthorRepo;

        public AuthorService(IAuthorRepository authorRepo,
                             IMapper mapper,
                             IBookAuthorRepository bookAuthorRepo)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
            _bookAuthorRepo = bookAuthorRepo;
        }

        public async Task CreateAsync(AuthorCreateDto model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var author = _mapper.Map<Author>(model);

            await _authorRepo.CreateAsync(author);
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var author = await _authorRepo.GetByIdAsync((int)id);

            if (author is null)
            {
                throw new NotFoundException("Data not found");
            }

            await _authorRepo.DeleteAsync(author);
        }

        public async Task EditAsync(int? id, AuthorEditDto model)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var author = await _authorRepo.GetByIdAsync((int)id);

            if (author is null)
            {
                throw new NotFoundException("Data not found");
            }

            await _authorRepo.EditAsync(_mapper.Map(model, author));
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var authors = await _authorRepo.FindAllWithIncludes()
                                           .Include(m => m.BookAuthors)
                                           .ThenInclude(m => m.Book)
                                           .ToListAsync();

            if (authors.Count() < 1)
            {
                throw new NotFoundException("Data not found");
            }

            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetByIdAsync(int? id)
        {
            var author = await _authorRepo.FindAllWithIncludes()
                                          .Include(m => m.BookAuthors)
                                          .ThenInclude(m => m.Book)
                                          .FirstOrDefaultAsync(m => m.Id == (int)id);

            if(author is null)
            {
                throw new NotFoundException("Data not found");
            }

            return _mapper.Map<AuthorDto>(author);
        }
    }
}
