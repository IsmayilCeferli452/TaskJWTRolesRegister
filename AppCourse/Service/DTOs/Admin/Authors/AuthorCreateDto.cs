using FluentValidation;
using Service.DTOs.Admin.Students;

namespace Service.DTOs.Admin.Authors
{
    public class AuthorCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<int> BookIds { get; set; }
    }


    public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
    {
        public AuthorCreateDtoValidator()
        {
            RuleFor(m => m.FullName)
                .NotEmpty()
                .WithMessage("FullName is required")
                .MaximumLength(50)
                .WithMessage("FullName cannot exceed 50 characters");

            RuleFor(m => m.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is invalid")
                .MaximumLength(50)
                .WithMessage("Email can be max 50 characters");

            RuleFor(m => m.Age)
                .NotEmpty()
                .WithMessage("Age is required")
                .GreaterThan(0)
                .WithMessage("Age must be greater than 0");

            RuleFor(m => m.BookIds)
                .NotEmpty()
                .WithMessage("At least one group is required");
        }
    }
}
