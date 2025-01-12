using Microsoft.EntityFrameworkCore;

namespace Server.Services;

public class ContactsDbContext(DbContextOptions<ContactsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().ToTable("Contacts");
        modelBuilder.Entity<Contact>().Property(c => c.Phone).HasMaxLength(15);
        modelBuilder.Entity<Contact>().Property(c => c.Name).HasMaxLength(50);
        modelBuilder.Entity<Contact>().Property(c => c.Salary).HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Contact>().HasData(new List<Contact>
        {
            new Contact { Name = "John Doe", DateOfBirth = new DateOnly(1980, 1, 1), Married = true, Phone = "+380563829166", Salary = 1000, Id = 1},
            new Contact { Name = "Jane Doe", DateOfBirth = new DateOnly(1985, 1, 1), Married = false, Phone = "+380594629166", Salary = 2000, Id = 2},
            new Contact { Name = "Sammy Doe", DateOfBirth = new DateOnly(1990, 1, 1), Married = true, Phone = "+380563829166", Salary = 3000, Id = 3 },
            new Contact { Name = "Sally Doe", DateOfBirth = new DateOnly(1995, 1, 1), Married = false, Phone = "+380888829166", Salary = 4000, Id = 4 },
            new Contact { Name = "Sandy Doe", DateOfBirth = new DateOnly(2000, 1, 1), Married = true, Phone = "+380563829166", Salary = 5000, Id = 5 },
            new Contact { Name = "Victor Van", DateOfBirth = new DateOnly(2005, 1, 1), Married = false, Phone = "+380563800566", Salary = 6000, Id = 6 },
            new Contact { Name = "Maria Red", DateOfBirth = new DateOnly(2010, 1, 1), Married = true, Phone = "+380568837666", Salary = 7000, Id = 7 },
            new Contact { Name = "Pan Cat", DateOfBirth = new DateOnly(2015, 1, 1), Married = false, Phone = "+380563826694", Salary = 8000, Id = 8 }
        });
    }
}