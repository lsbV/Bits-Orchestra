using FluentValidation;

namespace Server.Validators;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(50).Matches(@"^[a-zA-Z\s]*$").MinimumLength(3);
        RuleFor(c => c.DateOfBirth).NotEmpty().GreaterThan(new DateOnly(1900, 1, 1)).LessThan(new DateOnly(2020, 1, 1));
        RuleFor(c => c.Phone).NotEmpty().MaximumLength(15);
        RuleFor(c => c.Salary).NotEmpty().GreaterThan(1);
    }
}