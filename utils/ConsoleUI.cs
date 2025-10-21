namespace SanVicenteHospital.utils;

using SanVicenteHospital.models;
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

    public static void ShowDoctor(Doctor doctor)
    {
        Console.WriteLine($"\n🆔 ID: {doctor.Id}");
        Console.WriteLine($"👤 Name: {doctor.Name}");
        Console.WriteLine($"🆔 Identification: {doctor.Identification}");
        Console.WriteLine($"🎂 Age: {doctor.Age}");
        Console.WriteLine($"🏠 Address: {doctor.Address}");
        Console.WriteLine($"📞 Phone: {doctor.Phone}");
        Console.WriteLine($"✉️  Email: {doctor.Email}");
        Console.WriteLine($"🩺 Specialty: {doctor.Specialty}");
    }

    public static void ShowDoctorList(IEnumerable<Doctor> doctors)
    {
        foreach (var doctor in doctors)
            ShowDoctor(doctor);

        Console.WriteLine("\n-----------------------");
    }

    public static void ShowPatient(Patient patient)
    {
        Console.WriteLine($"\n🆔 ID: {patient.Id}");
        Console.WriteLine($"👤 Name: {patient.Name}");
        Console.WriteLine($"👤 Identification: {patient.Identification}");
        Console.WriteLine($"🎂 Age: {patient.Age}");
        Console.WriteLine($"🏠 Address: {patient.Address}");
        Console.WriteLine($"📞 Phone: {patient.Phone}");
        Console.WriteLine($"✉️  Email: {patient.Email}");
    }

    public static void ShowPatientList(IEnumerable<Patient> patients)
    {
        foreach (var patient in patients)
            ShowPatient(patient);

        Console.WriteLine("\n-----------------------");
    }

    public static void ShowAppointment(Appointment appointment)
    {
        Console.WriteLine($"🧍 Patient ID: {appointment.PatientId}");
        Console.WriteLine($"👨‍⚕️ Doctor ID: {appointment.DoctorId}");
        Console.WriteLine($"📅 Start Time: {appointment.StartTime:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"📅 End Time: {appointment.EndTime:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"💉 Service: {appointment.ServiceType}");
        Console.WriteLine($"🗒️ Reason: {appointment.Reason}");
    }

    public static void ShowAppointmentsList(
        List<Appointment> appointments,
        List<Patient> patients,
        List<Doctor> doctors)
    {
        Console.WriteLine("\n--- 📅  View Appointments ---");

        foreach (var a in appointments.OrderBy(a => a.StartTime))
        {
            var patient = patients.FirstOrDefault(p => p.Id == a.PatientId);
            var doctor = doctors.FirstOrDefault(d => d.Id == a.DoctorId);

            Console.WriteLine($"\n🆔 {a.Id}");
            Console.WriteLine($"🧍 Patient: {patient?.Name ?? "Unknown"} ({a.PatientId})");
            Console.WriteLine($"👨‍⚕️ Doctor: {doctor?.Name ?? "Unknown"} ({a.DoctorId})");
            Console.WriteLine($"📅 Start Time: {a.StartTime}");
            Console.WriteLine($"📅 End Time: {a.EndTime}");
            Console.WriteLine($"💉 Service: {a.ServiceType}");
            Console.WriteLine($"🗒️  Reason: {a.Reason}");
            Console.WriteLine($"📋 Status: {a.Status}");
        }

        Console.WriteLine("\n--- End of Appointments List ---");
    }

}