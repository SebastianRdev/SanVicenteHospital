namespace SanVicenteHospital.models;

using SanVicenteHospital.interfaces;

public class EmailLog : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Status { get; set; } = "Not sent";
    public DateTime DateSent { get; set; } = DateTime.Now;
    public string? ErrorMessage { get; set; }
}
