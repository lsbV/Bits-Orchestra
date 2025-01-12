public record DeleteContactCommand(int Id) : IRequest<DeleteContactResult>;

public record DeleteContactResult(bool Success, string? Error);

public class DeleteContactCommandHandler(ContactsDbContext dbContext)
    : IRequestHandler<DeleteContactCommand, DeleteContactResult>
{
    public async Task<DeleteContactResult> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await dbContext.Contacts.FindAsync(new object[] { request.Id }, cancellationToken);
        if (contact is null)
        {
            return new DeleteContactResult(false, "Contact not found");
        }
        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteContactResult(true, null);
    }
}