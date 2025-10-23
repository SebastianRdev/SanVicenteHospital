namespace SanVicenteHospital.services;

using SanVicenteHospital.models;
using SanVicenteHospital.models.Enums;
using SanVicenteHospital.repositories;
using SanVicenteHospital.interfaces;

/// Service that manages appointment-related business logic.
public class AppointmentService
{
    private readonly IRepository<Patient> _patientRepo;
    private readonly IRepository<Doctor> _doctorRepo;
    private readonly IRepository<Appointment> _appointmentRepo;
    private readonly EmailService _emailService;


    public AppointmentService(
        IRepository<Patient> patientRepo,
        IRepository<Doctor> doctorRepo,
        IRepository<Appointment> appointmentRepo,
        EmailService emailService)
    {
        _patientRepo = patientRepo;
        _doctorRepo = doctorRepo;
        _appointmentRepo = appointmentRepo;
        _emailService = emailService;
    }

    // Registers a new appointment interactively.
    public Appointment RegisterAppointment(Guid patientId, Guid doctorId, DateTime startTime, DateTime endTime, ServiceType serviceType, string reason)
    {
        // 🩺 Basic validations
        var patient = _patientRepo.GetById(patientId) ?? throw new KeyNotFoundException("Patient not found");
        var doctor = _doctorRepo.GetById(doctorId) ?? throw new KeyNotFoundException("Doctor not found");

        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time");

        // Validate appointment interlocking for the doctor
        bool overlapsDoctor = _appointmentRepo.GetAll().Any(a =>
            a.DoctorId == doctorId &&
            ((startTime >= a.StartTime && startTime < a.EndTime) ||
             (endTime > a.StartTime && endTime <= a.EndTime) ||
             (startTime <= a.StartTime && endTime >= a.EndTime))
        );

        if (overlapsDoctor)
            throw new InvalidOperationException("❌ The doctor already has an appointment in that time range");

        // Validate appointment interlocking for the patient
        bool overlapsPatient = _appointmentRepo.GetAll().Any(a =>
            a.PatientId == patientId &&
            ((startTime >= a.StartTime && startTime < a.EndTime) ||
             (endTime > a.StartTime && endTime <= a.EndTime) ||
             (startTime <= a.StartTime && endTime >= a.EndTime))
        );

        if (overlapsPatient)
            throw new InvalidOperationException("❌ The patient already has an appointment in that time range");

        var appointment = new Appointment(patientId, doctorId, startTime, endTime, serviceType, reason);
        _appointmentRepo.Add(appointment);

        var emailLog = _emailService.SendAppointmentConfirmation(
            patient.Email ?? "Unknown",
            patient.Name ?? "Unknown",
            doctor.Name ?? "Unknown",
            startTime,
            endTime,
            reason
        );

        return appointment;
    }

    // Returns all appointments from the repository.
    public List<Appointment> ViewAppointments()
    {
        return _appointmentRepo.GetAll().ToList();
    }

    public void CancelAppointment(Guid appointmentId, string? reason = null)
    {
        var appointment = _appointmentRepo.GetById(appointmentId)
            ?? throw new KeyNotFoundException("Appointment not found");

        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("Appointment is already cancelled.");

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = reason ?? "No reason provided";
        appointment.CancellationDate = DateTime.Now;

        _appointmentRepo.Update(appointment);
    }


    // Updates an existing appointment with new values.
    public void UpdateAppointment(
        Guid appointmentId,
        Guid? newPatientId = null,
        Guid? newDoctorId = null,
        DateTime? newStartTime = null,
        DateTime? newEndTime = null,
        ServiceType? newService = null,
        string? newReason = null)
    {
        var appointment = _appointmentRepo.GetById(appointmentId)
            ?? throw new KeyNotFoundException("Appointment not found");

        if (newPatientId.HasValue)
        {
            var patient = _patientRepo.GetById(newPatientId.Value)
                ?? throw new KeyNotFoundException("Patient not found");
            appointment.PatientId = patient.Id;
        }

        if (newDoctorId.HasValue)
        {
            var doctor = _doctorRepo.GetById(newDoctorId.Value)
                ?? throw new KeyNotFoundException("Doctor not found");
            appointment.DoctorId = doctor.Id;
        }

        if (newStartTime.HasValue)
        {
            if (newStartTime.Value < DateTime.Now)
                throw new ArgumentException("Start time cannot be in the past");
            appointment.StartTime = newStartTime.Value;
        }

        if (newEndTime.HasValue)
        {
            if (newEndTime.Value <= appointment.StartTime)
                throw new ArgumentException("End time must be after start time");
            appointment.EndTime = newEndTime.Value;
        }

        if (newService.HasValue)
            appointment.ServiceType = newService.Value;

        if (!string.IsNullOrWhiteSpace(newReason))
            appointment.Reason = newReason;

        _appointmentRepo.Update(appointment);
    }

    // Gets all patients from the repository.
    public List<Patient> GetAllPatients()
    {
        return _patientRepo.GetAll().ToList();
    }

    // Gets all active doctors from the repository.
    public List<Doctor> GetAllDoctors()
    {
        return _doctorRepo.GetAll().Where(d => d.IsActive).ToList();
    }

    // Returns a single appointment by ID.
    public Appointment GetAppointmentById(Guid id)
    {
        var appointment = _appointmentRepo.GetById(id);
        if (appointment == null)
            throw new KeyNotFoundException($"Appointment with ID {id} not found");
        
        return appointment;
    }

    public void ChangeAppointmentStatus(Guid appointmentId, AppointmentStatus newStatus, string? notes = null)
    {
        var appointment = _appointmentRepo.GetById(appointmentId)
            ?? throw new KeyNotFoundException("Appointment not found");

        // Validate workflow logic (e.g., cannot mark as Completed if not InProgress)
        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("Cannot change status of a cancelled appointment");

        appointment.Status = newStatus;

        if (!string.IsNullOrWhiteSpace(notes))
            appointment.Notes = notes;

        _appointmentRepo.Update(appointment);
    }

    public void ViewEmailHistory()
    {
        _emailService.ViewEmailHistory();
    }

    public List<Appointment> GetAppointmentsByPatient(Guid patientId)
    {
        var patient = _patientRepo.GetById(patientId);
        if (patient == null)
            throw new KeyNotFoundException($"⚠️  Patient with ID {patientId} not found");

        return _appointmentRepo.GetAll()
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.StartTime)
            .ToList();
    }

    public List<Appointment> GetAppointmentsByDoctor(Guid doctorId)
    {
        var doctor = _doctorRepo.GetById(doctorId);
        if (doctor == null)
            throw new KeyNotFoundException($"⚠️  Doctor with ID {doctorId} not found");
        
        return _appointmentRepo.GetAll()
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.StartTime)
            .ToList();
    }

    public List<Appointment> GetAppointmentsByDate(DateTime date)
    {

        return _appointmentRepo.GetAll()
            .Where(a => a.StartTime.Date == date.Date)
            .OrderBy(a => a.StartTime)
            .ToList();
    }

    public List<Appointment> GetAppointmentsByStatus(AppointmentStatus status)
    {
        return _appointmentRepo.GetAll()
            .Where(a => a.Status == status)
            .OrderBy(a => a.StartTime)
            .ToList();
    }

}