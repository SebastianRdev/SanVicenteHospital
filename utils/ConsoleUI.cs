namespace SanVicenteHospital.utils;

using SanVicenteHospital.interfaces;

// Utility class for displaying menus in the SanVicenteHospital system console.
// Implements the IConsoleUI interface.
public class ConsoleUI : IConsoleUI
{
    // Displays the main menu of the application on the console.
    public static void ShowMainMenu()
    {
        Console.WriteLine("\n📋 Main Menu:");
        Console.WriteLine("1️⃣  Patients Module");
        Console.WriteLine("2️⃣  Doctors Module");
        Console.WriteLine("3️⃣  Appointments Module");
        Console.WriteLine("4️⃣  Exit 🚪");
    }

    // Displays the ShowPatientMainMenu menu on the console.
    public static void ShowPatientMainMenu()
    {
        Console.WriteLine("\n📋 Patient Main Menu:");
        Console.WriteLine("1️⃣  Patient CRUD");
        Console.WriteLine("2️⃣  Back to Main Menu 🔙");
    }

    // Displays the customerCRUD menu on the console.
    public static void ShowPatientCRUD()
    {
        Console.WriteLine("\n📋 Patient CRUD:");
        Console.WriteLine("1️⃣  Register Patient");
        Console.WriteLine("2️⃣  View Patients");
        Console.WriteLine("3️⃣  Update a Patient");
        Console.WriteLine("4️⃣  Delete a Patient");
        Console.WriteLine("5️⃣  Back to Main Menu 🔙");
    }

    // Displays the ShowDoctorMainMenu menu on the console.
    public static void ShowDoctorMainMenu()
    {
        Console.WriteLine("\n📋 Doctor Main Menu:");
        Console.WriteLine("1️⃣  Doctor CRUD");
        Console.WriteLine("2️⃣  View all doctors by specialty");
        Console.WriteLine("3️⃣  Back to Main Menu 🔙");
    }

    // Displays the DoctorCRUD menu on the console.
    public static void ShowDoctorCRUD()
    {
        Console.WriteLine("\n📋 Doctor CRUD:");
        Console.WriteLine("1️⃣  Register Doctor");
        Console.WriteLine("2️⃣  View Doctors");
        Console.WriteLine("3️⃣  Update a Doctor");
        Console.WriteLine("4️⃣  Delete a Doctor");
        Console.WriteLine("5️⃣  Back to Main Menu 🔙");
    }

    // Displays the AppointmentsMainMenu on the console.
    public static void ShowAppointmentsMainMenu()
    {
        Console.WriteLine("\n📋 Appointments Main Menu:");
        Console.WriteLine("1️⃣  Appointments CRUD");
        Console.WriteLine("2️⃣  Change the status of an appointment");
        Console.WriteLine("3️⃣  View medical appointments filtered by:");
        Console.WriteLine("4️⃣  View email history 📧");
        Console.WriteLine("5️⃣  Back to Main Menu 🔙");
    }

    // Displays the AppointmentsCRUD on the console.
    public static void ShowAppointmentsCRUD()
    {
        Console.WriteLine("\n📋 Appointments CRUD:");
        Console.WriteLine("1️⃣  Register appointment");
        Console.WriteLine("2️⃣  View appointments");
        Console.WriteLine("3️⃣  Update a appointment");
        Console.WriteLine("4️⃣  Delete a appointment");
        Console.WriteLine("5️⃣  Back to Appointments Main Menu 🔙");
    }

    // Displays the Appointments Filtered By on the console.
    public static void ShowAppointmentsByMenu()
    {
        Console.WriteLine("\n📋 View Medical Appointments Filtered By:");
        Console.WriteLine("1️⃣  View appointments by patient");
        Console.WriteLine("2️⃣  View appointments by doctor");
        Console.WriteLine("3️⃣  View appointments by date");
        Console.WriteLine("4️⃣  View appointments by status");
        Console.WriteLine("5️⃣  Back to Appointments Main Menu 🔙");
    }
}