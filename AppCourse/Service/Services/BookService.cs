using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.BookAuthors;
using Service.DTOs.Admin.Books;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    [Authorize]
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IMapper _mapper;
        private readonly IBookAuthorRepository _bookAuthorRepo;
        private readonly IAuthorRepository _authorRepo;

        public BookService(IBookRepository bookRepo,
                           IMapper mapper,
                           IBookAuthorRepository bookAuthorRepo,
                           IAuthorRepository authorRepo)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
            _bookAuthorRepo = bookAuthorRepo;
            _authorRepo = authorRepo;
        }

        public async Task CreateAsync(BookCreateDto model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var book = _mapper.Map<Book>(model);

            await _bookRepo.CreateAsync(book);

            foreach (var id in model.AuthorIds)
            {
                await _bookAuthorRepo.CreateAsync(new BookAuthors
                {
                    BookId = book.Id,
                    AuthorId = id
                });
            }
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var book = await _bookRepo.GetByIdAsync((int)id);

            if (book is null)
            {
                throw new NotFoundException("Data not found");
            }

            await _bookRepo.DeleteAsync(book);
        }

        public async Task EditAsync(int? id, BookEditDto model)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var book = await _bookRepo.GetByIdAsync((int)id);

            if (book is null)
            {
                throw new NotFoundException("Data not found");
            }

            await _bookRepo.EditAsync(_mapper.Map(model, book));
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _bookRepo.FindAllWithIncludes()
                                       .Include(m => m.BookAuthors)
                                       .ThenInclude(m => m.Author)
                                       .ToListAsync();

            if (books.Count() < 1)
            {
                throw new NotFoundException("Data not found");
            }

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var book = await _bookRepo.FindAllWithIncludes()
                                      .Include(m => m.BookAuthors)
                                      .ThenInclude(m => m.Author)
                                      .FirstOrDefaultAsync(m => m.Id == (int)id);

            if (book is null)
            {
                throw new NotFoundException("Data not found");
            }

            return _mapper.Map<BookDto>(book);
        }

        public async Task AddGroupAsync(BookAuthorCreateDto model)
        {
            if (model is null)
            {
                throw new ArgumentNullException();
            }

            if (await _bookRepo.GetByIdAsync(model.BookId) is null)
            {
                throw new NotFoundException("Data not found");
            }

            if(await _authorRepo.GetByIdAsync(model.AuthorId) is null)
            {
                throw new NotFoundException("Data not found");
            }

            await _bookAuthorRepo.CreateAsync(_mapper.Map<BookAuthors>(model));
        }
    }
}
