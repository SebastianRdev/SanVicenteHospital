using SanVicenteHospital.menus;
using SanVicenteHospital.models;
using SanVicenteHospital.repositories;
using SanVicenteHospital.services;
using SanVicenteHospital.seeders;

public class Program
{
    static void Main()
    {
        var patientRepo = new RepositoryDict<Patient>();
        var doctorRepo = new RepositoryDict<Doctor>();
        var appointmentRepo = new Repository<Appointment>();
        var emailRepo = new Repository<EmailLog>();

        var emailService = new EmailService(emailRepo);
        var patientService = new PatientService(patientRepo, doctorRepo);
        var doctorService = new DoctorService(doctorRepo, patientRepo);
        var appointmentService = new AppointmentService(patientRepo, doctorRepo, appointmentRepo, emailService);

        // 🌱 Initialize example dates
        DatabaseSeeder.Seed(patientService, doctorService, appointmentService);

        // 🖥️ Initialize the main menu
        MainMenu.Menu();
    }
}
