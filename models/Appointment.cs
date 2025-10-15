namespace SanVicenteHospital.models;

using SanVicenteHospital.interfaces;
using SanVicenteHospital.models.Enums;

public class Appointment : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Reason { get; set; } = string.Empty;

    public Guid PetId { get; set; }

    public Guid VeterinarianId { get; set; }

    public ServiceType ServiceType { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public string Notes { get; set; } = string.Empty;

    public string? CancellationReason { get; set; }
    public DateTime? CancellationDate { get; set; }


    public Appointment(Guid petId, Guid veterinarianId, DateTime startTime, DateTime endTime, ServiceType serviceType, string reason)
    {
        PetId = petId;
        VeterinarianId = veterinarianId;
        StartTime = startTime;
        EndTime = endTime;
        ServiceType = serviceType;
        Reason = reason;
        Status = AppointmentStatus.Scheduled;
    }
}
