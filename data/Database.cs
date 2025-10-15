namespace SanVicenteHospital.data;

using SanVicenteHospital.models;

// Class that simulates the in-memory database for the SanVicenteHospital system.
// Stores lists and dictionaries of clients, pets, and veterinarians.
public class Database
{
    // Dictionary of patients indexed by Guid for quick access.
    public static Dictionary<Guid, Patient> PatientsDict { get; } = new();

    // Dictionary of doctors indexed by Guid for quick access.
    public static Dictionary<Guid, Doctor> DoctorsDict { get; } = new();

    // List of appointments
    public static List<Appointment> Appointments { get; } = new();
    
    // List of logs
    public static List<EmailLog> EmailLogs { get; } = new();
}
