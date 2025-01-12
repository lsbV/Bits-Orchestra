public record UpdateContactCommand(int Id, Contact Contact) : IRequest<UpdateContactResult>;

public record UpdateContactResult(bool Success, string? Error);

public class UpdateContactCommandHandler(ContactsDbContext dbContext)
    : IRequestHandler<UpdateContactCommand, UpdateContactResult>
{
    public async Task<UpdateContactResult> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Contacts.FindAsync(new object[] { request.Id }, cancellationToken);
        if (existing is null)
        {
            return new UpdateContactResult(false, "Contact not found");
        }
        existing.Name = request.Contact.Name;
        existing.DateOfBirth = request.Contact.DateOfBirth;
        existing.Married = request.Contact.Married;
        existing.Phone = request.Contact.Phone;
        existing.Salary = request.Contact.Salary;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateContactResult(true, null);
    }
}