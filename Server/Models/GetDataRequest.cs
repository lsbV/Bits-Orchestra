namespace Server.Models;

public class GetDataRequest
{
    public required SortInfo? Sort { get; set; }
    public required Pagination Pagination { get; set; }
    public required Filter[] Filters { get; set; }
}