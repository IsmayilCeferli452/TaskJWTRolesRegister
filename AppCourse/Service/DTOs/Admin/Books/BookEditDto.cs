using FluentValidation;

namespace Service.DTOs.Admin.Books
{
    public class BookEditDto
    {
        public string Name { get; set; }
    }


    public class BookEditDtoValidator : AbstractValidator<BookEditDto>
    {
        public BookEditDtoValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters");
        }
    }
}
