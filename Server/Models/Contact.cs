namespace Server.Models;

public class Contact
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required bool Married { get; set; }
    public required string Phone { get; set; }
    public required decimal Salary { get; set; }
}