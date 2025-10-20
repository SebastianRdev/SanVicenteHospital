namespace SanVicenteHospital.models;

using SanVicenteHospital.interfaces;
using SanVicenteHospital.models.Enums;

public class Appointment : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Reason { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public ServiceType ServiceType { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public string Notes { get; set; } = string.Empty;

    public string? CancellationReason { get; set; }
    public DateTime? CancellationDate { get; set; }


    public Appointment(Guid patientId, Guid doctorId, DateTime startTime, DateTime endTime, ServiceType serviceType, string reason)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        StartTime = startTime;
        EndTime = endTime;
        ServiceType = serviceType;
        Reason = reason;
        Status = AppointmentStatus.Scheduled;
    }
}
