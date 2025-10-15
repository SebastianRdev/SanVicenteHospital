namespace SanVicenteHospital.models;

public class Person
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public int Identification { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}