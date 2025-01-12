using Microsoft.EntityFrameworkCore;

namespace Server.Extensions;

internal static class AppExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<ContactsDbContext>()!;
        db.Database.Migrate();
    }

}