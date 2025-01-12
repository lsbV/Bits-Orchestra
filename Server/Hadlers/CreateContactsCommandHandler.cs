using FluentValidation.Results;

public record CreateContactsCommand(IFormFile file) : IRequest<CreateContactsResult>;

public record CreateContactsResult(bool Success, List<ValidationFailure>? Errors);

public class CreateContactsCommandHandler(ContactsDbContext dbContext, ContactValidator contactValidator) : IRequestHandler<CreateContactsCommand, CreateContactsResult>
{
    public async Task<CreateContactsResult> Handle(CreateContactsCommand request, CancellationToken cancellationToken)
    {
        await using var readStream = request.file.OpenReadStream();
        using var reader = new StreamReader(readStream);
        await reader.ReadLineAsync(cancellationToken); // skip header
        var contacts = new List<Contact>();
        while ((await reader.ReadLineAsync(cancellationToken)) is { Length: > 0 } line)
        {
            var contact = line.ToContact();
            var validationResult = contactValidator.Validate(contact);
            if (!validationResult.IsValid)
            {
                return new CreateContactsResult(false, validationResult.Errors);
            }
            contacts.Add(contact);
        }
        await dbContext.Contacts.AddRangeAsync(contacts, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateContactsResult(true, null);

    }
}