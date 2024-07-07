using FluentValidation;
using Service.DTOs.Admin.Authors;

namespace Service.DTOs.Admin.Books
{
    public class BookCreateDto
    {
        public string Name { get; set; }
        public List<int> AuthorIds { get; set; }
    }


    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters");

            RuleFor(m => m.AuthorIds)
                .NotEmpty()
                .WithMessage("At least one group is required");
        }
    }
}
