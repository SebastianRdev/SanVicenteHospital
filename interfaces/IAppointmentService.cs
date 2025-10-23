namespace SanVicenteHospital.interfaces;

using SanVicenteHospital.models;
using SanVicenteHospital.models.Enums;

public interface IAppointmentService
{
    Appointment RegisterAppointment(Guid patientId, Guid doctorId, DateTime startTime, DateTime endTime, ServiceType serviceType, string reason);

    List<Appointment> ViewAppointments();

    void UpdateAppointment(
        Guid appointmentId,
        Guid? newPatientId = null,
        Guid? newDoctorId = null,
        DateTime? newStartTime = null,
        DateTime? newEndTime = null,
        ServiceType? newService = null,
        string? newReason = null
    );

    void CancelAppointment(Guid appointmentId, string? reason = null);

    Appointment GetAppointmentById(Guid id);

    void ChangeAppointmentStatus(Guid appointmentId, AppointmentStatus newStatus, string? notes = null);

    List<Patient> GetAllPatients();

    List<Doctor> GetAllDoctors();

    void ViewEmailHistory();

    List<Appointment> GetAppointmentsByPatient(Guid patientId);

    List<Appointment> GetAppointmentsByDoctor(Guid doctorId);

    List<Appointment> GetAppointmentsByDate(DateTime date);

    List<Appointment> GetAppointmentsByStatus(AppointmentStatus status);
}
