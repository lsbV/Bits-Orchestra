using System.Globalization;

namespace Server.Extensions;

internal static class StringExtensions
{
    public static Contact ToContact(this string line)
    {
        var parts = line.Split(',');
        return new Contact
        {
            Name = parts[0],
            DateOfBirth = DateOnly.Parse(parts[1], CultureInfo.InvariantCulture),
            Married = bool.Parse(parts[2]),
            Phone = parts[3],
            Salary = decimal.Parse(parts[4])
        };
    }
}