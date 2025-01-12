using System.Globalization;
using Microsoft.EntityFrameworkCore;

public record GetDataQuery(GetDataRequest Request) : IRequest<GetDataResponse>;
public class GetDataResponse
{
    public required int TotalCount { get; set; }
    public required List<Contact> Contacts { get; set; }
}
public class GetDataQueryHandler(ContactsDbContext dbContext) : IRequestHandler<GetDataQuery, GetDataResponse>
{
    public async Task<GetDataResponse> Handle(GetDataQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Contacts.AsQueryable();
        var formatProvider = CultureInfo.InvariantCulture;

        foreach (var filter in request.Request.Filters)
        {
            query = filter.By switch
            {
                nameof(Contact.Name) => query.Where(c => c.Name.Contains(filter.Value)),
                nameof(Contact.DateOfBirth) => query.Where(c =>
                    c.DateOfBirth == DateOnly.Parse(filter.Value, formatProvider)),
                nameof(Contact.Married) => query.Where(c => c.Married == bool.Parse(filter.Value)),
                nameof(Contact.Phone) => query.Where(c => c.Phone.Contains(filter.Value)),
                nameof(Contact.Salary) => query.Where(c => c.Salary == decimal.Parse(filter.Value, formatProvider)),
                _ => query
            };
        }
        if (request.Request.Sort is not null)
        {
            query = request.Request.Sort.By switch
            {
                nameof(Contact.Name) => request.Request.Sort.Order == Order.Asc
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name),
                nameof(Contact.DateOfBirth) => request.Request.Sort.Order == Order.Asc
                    ? query.OrderBy(c => c.DateOfBirth)
                    : query.OrderByDescending(c => c.DateOfBirth),
                nameof(Contact.Married) => request.Request.Sort.Order == Order.Asc
                    ? query.OrderBy(c => c.Married)
                    : query.OrderByDescending(c => c.Married),
                nameof(Contact.Phone) => request.Request.Sort.Order == Order.Asc
                    ? query.OrderBy(c => c.Phone)
                    : query.OrderByDescending(c => c.Phone),
                nameof(Contact.Salary) => request.Request.Sort.Order == Order.Asc
                    ? query.OrderBy(c => c.Salary)
                    : query.OrderByDescending(c => c.Salary),
                _ => query
            };
        }

        var count = await query.CountAsync(cancellationToken);
        var data = await query.Skip(((int)request.Request.Pagination.Page - 1) * (int)request.Request.Pagination.Count)
            .Take((int)request.Request.Pagination.Count)
            .ToListAsync(cancellationToken);

        return new GetDataResponse { TotalCount = count, Contacts = data };
    }
}


