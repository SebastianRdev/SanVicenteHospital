namespace SanVicenteHospital.menus;

using SanVicenteHospital.interfaces;
using SanVicenteHospital.repositories;
using SanVicenteHospital.services;
using SanVicenteHospital.models;
using SanVicenteHospital.utils;

public class MainMenu
{
    // Crear repositorios una sola vez
    private static readonly IRepository<Patient> _patientRepo = new RepositoryDict<Patient>();
    private static readonly IRepository<Doctor> _doctorRepo = new RepositoryDict<Doctor>();
    private static readonly IRepository<Appointment> _appointmentRepo = new Repository<Appointment>();
    private static readonly IRepository<EmailLog> _emailRepo = new Repository<EmailLog>();

    // Crear servicios una sola vez
    private static readonly PatientService _patientService = new(_patientRepo, _doctorRepo);
    private static readonly DoctorService _doctorService = new(_doctorRepo, _patientRepo);
    private static readonly EmailService _emailService = new EmailService(_emailRepo);
    private static readonly AppointmentService _appointmentService = new(_patientRepo, _doctorRepo, _appointmentRepo, _emailService);

    // Create menus by passing dependencies
    private static readonly PatientMenu _patientMenu = new(_patientService);
    private static readonly DoctorMenu _doctorMenu = new(_doctorService);
    private static readonly AppointmentMenu _appointmentMenu = new(_appointmentService, _emailService);

    public static void Menu()
    {
        Console.WriteLine("\n🐾 Welcome to SanVicenteHospital System 🏥");
        Console.WriteLine("-----------------------------------");

        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowMainMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        _patientMenu.PatientMainMenu();
                        continue;
                    case 2:
                        _doctorMenu.DoctorMainMenu();
                        continue;
                    case 3:
                        _appointmentMenu.AppointmentMainMenu();
                        continue;
                    case 4:
                        Console.WriteLine("\n👋 Thanks for using SanVicenteHospital System. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("\n⚠️  Invalid choice. Please try again");
                        continue;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Invalid input. Please enter a valid number.");
                continue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n⚠️  Unexpected error: {ex.Message}");
                continue;
            }
            break;
        }
    }
}
