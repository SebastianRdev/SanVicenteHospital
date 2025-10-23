namespace SanVicenteHospital.menus;

using SanVicenteHospital.utils;
using SanVicenteHospital.services;
using SanVicenteHospital.interfaces;
using SanVicenteHospital.models.Enums;

public class AppointmentMenu
{
    private readonly AppointmentService _appointmentService;

    public AppointmentMenu(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public void AppointmentMainMenu()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowAppointmentsMainMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AppointmentsCRUD();
                        continue;
                    case 2:
                        ChangeAppointmentStatusUI();
                        WaitForKey();
                        continue;
                    case 3:
                        ViewAppointmentsFilteredByMenuUI();
                        continue;
                    case 4:
                        _appointmentService.ViewEmailHistory();
                        WaitForKey();
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to main menu");
                        break;
                    default:
                        Console.WriteLine("\n⚠️  Invalid choice. Please try again");
                        continue;
                }
            }
            catch
            {
                Console.WriteLine("\n❌ Invalid input. Please enter a number");
                WaitForKey();
                continue;
            }
            break;
        }
    }

    public void AppointmentsCRUD()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowAppointmentsCRUD();
                Console.Write("\n👉 Enter your choice: ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("\n❌ Invalid input. Please enter a number");
                    continue;
                }
                switch (choice)
                {
                    case 1:
                        RegisterAppointmentUI();
                        WaitForKey();
                        continue;
                    case 2:
                        ViewAppointmentsUI();
                        WaitForKey();
                        continue;
                    case 3:
                        UpdateAppointmentUI();
                        WaitForKey();
                        continue;
                    case 4:
                        RemoveAppointmentUI();
                        WaitForKey();
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to main menu");
                        break;
                    default:
                        Console.WriteLine("\n⚠️  Invalid choice. Please try again");
                        continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ {ex.Message}");
                WaitForKey();
                continue;
            }
            break;
        }
    }

    private void WaitForKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void RegisterAppointmentUI()
    {
        try
        {
            Console.WriteLine("\n--- 🗓️  Register Appointment ---");

            var patients = _appointmentService.GetAllPatients();
            if (!Validator.IsExist(patients, "⚠️  No patients registered. Please register a patient first")) return;

            var doctors = _appointmentService.GetAllDoctors();
            if (!Validator.IsExist(doctors, "⚠️  No doctors registered. Please register one first")) return;

            // Show available patients
            Console.WriteLine("\n --- Patient List ---");
            foreach (var p in patients)
                Console.WriteLine($"ID: {p.Id} | Name: {p.Name}");

            var patientId = Validator.ValidateGuid("\nEnter Patient ID: ");

            // 🩺 Show available doctors
            Console.WriteLine("\n👨‍⚕️ --- Doctor List ---");
            foreach (var v in doctors)
                Console.WriteLine($"ID: {v.Id} | Name: {v.Name} | Specialty: {v.Specialty}");

            var doctorId = Validator.ValidateGuid("\nEnter Doctor ID: ");

            // 📅 Appointment date
            DateTime startTime;
            while (true)
            {
                string dateInput = Validator.ValidateContent("Enter appointment start time (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(dateInput, out startTime))
                {
                    if (startTime <= DateTime.Now)
                        Console.WriteLine("⚠️  The appointment start time must be in the future");
                    else break;
                }
                else
                {
                    Console.WriteLine("⚠️  Invalid date format. Example: 2025-10-15 14:30");
                }
            }

            DateTime endTime;
            while (true)
            {
                string dateInput = Validator.ValidateContent("Enter appointment end time (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(dateInput, out endTime))
                {
                    if (endTime <= startTime)
                        Console.WriteLine("⚠️  The appointment end time must be after the start time");
                    else break;
                }
                else
                {
                    Console.WriteLine("⚠️  Invalid date format. Example: 2025-10-15 14:30");
                }
            }

            // 🩸 Available services
            var serviceType = Validator.ValidateServiceType();
            
            // 📝 Reason
            string reason = Validator.ValidateContent("Enter appointment reason: ");

            // ✅ Register appointment
            var appointment = _appointmentService.RegisterAppointment(patientId, doctorId, startTime, endTime, serviceType, reason);
            Console.WriteLine($"\n✅ Appointment registered successfully with ID: {appointment.Id}");
            Console.WriteLine("📧 A confirmation email has been sent to the patient.");
            
        }
        catch (FormatException)
        {
            Console.WriteLine("❌ Invalid input format. Please enter the data correctly.");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"❌ {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"⚠️  {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error: {ex.Message}");
        }
    }

    private void ViewAppointmentsUI()
    {
        var appointments = _appointmentService.ViewAppointments().OrderBy(a => a.StartTime).ToList();
        if (!Validator.IsExist(appointments, "⚠️  No appointments registered")) return;

        ConsoleUI.ShowAppointmentsList(appointments,_appointmentService.GetAllPatients(),_appointmentService.GetAllDoctors());
    }

    private void UpdateAppointmentUI()
    {
        Console.WriteLine("\n--- ✏️  Update Appointment ---");

        try
        {
            var appointments = _appointmentService.ViewAppointments().OrderBy(a => a.StartTime).ToList();
            if (!Validator.IsExist(appointments, "⚠️  No appointments registered")) return;

            foreach (var a in appointments)
            {
                Console.WriteLine($"\n🆔 {a.Id} | 🩺 {a.ServiceType} | 📅 {a.StartTime} : {a.EndTime} | 📋 Status: {a.Status}");
            }

            var appointmentId = Validator.ValidateGuid("\nEnter Appointment ID: ");

            // Get current appointment
            var appointment = _appointmentService.GetAppointmentById(appointmentId);
            if (!Validator.IsExist(appointment, "❌ No appointment found with that ID")) return;

            Console.WriteLine($"\nCurrent appointment details:");
            ConsoleUI.ShowAppointment(appointment);

            Console.WriteLine("\nUpdate fields (y/n):");

            Guid? newPatientId = appointment.PatientId;
            Guid? newDoctorId = appointment.DoctorId;
            DateTime newStartTime = appointment.StartTime;
            DateTime newEndTime = appointment.EndTime;
            ServiceType newService = appointment.ServiceType;
            string newReason = appointment.Reason;

            if (Validator.AskYesNo("Change patient? (y/n): "))
            {
                var patients = _appointmentService.GetAllPatients();
                if (!Validator.IsExist(patients, "⚠️  No patients available. Please register one first")) return;

                Console.WriteLine("\n--- Patient List ---");
                foreach (var p in patients)
                    Console.WriteLine($"ID: {p.Id} | Name: {p.Name}");

                newPatientId = Validator.ValidateGuid("\nEnter Patient ID: ");
            }
            
            if (Validator.AskYesNo("Change doctor? (y/n): "))
            {
                var doctors = _appointmentService.GetAllDoctors();
                if (!Validator.IsExist(doctors, "⚠️  No doctors available. Please register one first")) return;

                Console.WriteLine("\n--- Doctor List ---");
                foreach (var d in doctors)
                    Console.WriteLine($"ID: {d.Id} | Name: {d.Name} | Specialty: {d.Specialty}");

                newDoctorId = Validator.ValidateGuid("\nEnter Doctor ID: ");
            }
            
            if (Validator.AskYesNo("Change start time? (y/n): "))
            {
                string startTimeInput = Validator.ValidateContent("Enter new start time (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(startTimeInput, out DateTime startTime))
                    newStartTime = startTime;
            }
            
            if (Validator.AskYesNo("Change end time? (y/n): "))
            {
                string endTimeInput = Validator.ValidateContent("Enter new end time (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(endTimeInput, out DateTime endTime))
                    newEndTime = endTime;
                else
                    Console.WriteLine("⚠️  Invalid date format. Date not changed");
            }

            if (Validator.AskYesNo("Change service type? (y/n): "))
            {
                newService = Validator.ValidateServiceType();
            }
            
            if (Validator.AskYesNo("Change reason? (y/n): "))
                newReason = Validator.ValidateContent("📝 Enter new reason: ");

            _appointmentService.UpdateAppointment(
                appointmentId,
                newPatientId,
                newDoctorId,
                newStartTime,
                newEndTime,
                newService,
                newReason
            );

            Console.WriteLine("\n✅ Appointment updated successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("⚠️  Invalid format. Please enter valid data");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("❌ Appointment not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error updating appointment: {ex.Message}");
        }
    }

    private void RemoveAppointmentUI()
    {
        Console.WriteLine("\n--- ❌ Remove Appointment ---");

        var appointments = _appointmentService.ViewAppointments().OrderBy(a => a.StartTime).ToList();
        if (!Validator.IsExist(appointments, "⚠️  No appointments registered")) return;

        foreach (var a in appointments)
        {
            Console.WriteLine($"\n🆔 {a.Id} | 🩺 {a.ServiceType} | 📅 {a.StartTime} : {a.EndTime} | 📋 Status: {a.Status}");
        }

        var appointmentId = Validator.ValidateGuid("\nEnter Appointment ID: ");

        try
        {
            _appointmentService.CancelAppointment(appointmentId);
            Console.WriteLine("✅ Appointment cancelled successfully!");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("⚠️  Appointment not found");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error removing appointment: {ex.Message}");
        }
    }

    private void ChangeAppointmentStatusUI()
    {
        Console.WriteLine("\n--- 🔄 Change Appointment Status ---");

        var appointments = _appointmentService.ViewAppointments().OrderBy(a => a.StartTime).ToList();
        if (!Validator.IsExist(appointments, "⚠️  No appointments registered")) return;

       ViewAppointmentsUI();

        var appointmentId = Validator.ValidateGuid("\nEnter Appointment ID to change status: ");

        var appointment = _appointmentService.GetAppointmentById(appointmentId);

        if (!Validator.IsExist(appointment, "❌ Appointment not found")) return;

        Console.WriteLine($"\nCurrent status: {appointment.Status}");
        var newStatus = Validator.ValidateAppointmentStatus();

        string? notes = null;
        if (Validator.AskYesNo("Add notes to this status change? (y/n): "))
            notes = Validator.ValidateContent("🗒️  Enter notes: ");
        try
        {
            _appointmentService.ChangeAppointmentStatus(appointmentId, newStatus, notes);
            Console.WriteLine("\n✅ Appointment status updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error changing status: {ex.Message}");
        }
    }

    private void ViewAppointmentsFilteredByMenuUI()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowAppointmentsByMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ViewAppointmentsByPatientUI();
                        WaitForKey();
                        continue;
                    case 2:
                        ViewAppointmentsByDoctorUI();
                        WaitForKey();
                        continue;
                    case 3:
                        ViewAppointmentsByDateUI();
                        WaitForKey();
                        continue;
                    case 4:
                        ViewAppointmentsByStatusUI();
                        WaitForKey();
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to main menu");
                        break;
                    default:
                        Console.WriteLine("\n⚠️  Invalid choice. Please try again");
                        continue;
                }
            }
            catch
            {
                Console.WriteLine("\n❌ Invalid input. Please enter a number");
                WaitForKey();
                continue;
            }
            break;
        }
    }

    private void ViewAppointmentsByPatientUI()
    {
        Console.WriteLine("\n--- 🧍 View Appointments by Patient ---");

        var patients = _appointmentService.GetAllPatients();
        if (!Validator.IsExist(patients, "⚠️  No patients available. Please register one first")) return;

        Console.WriteLine("\n--- Patient List ---");
        foreach (var p in patients)
            Console.WriteLine($"ID: {p.Id} | Name: {p.Name}");

        var patientId = Validator.ValidateGuid("\nEnter Patient ID: ");

        var appointments = _appointmentService.GetAppointmentsByPatient(patientId);
        if (!Validator.IsExist(appointments, "⚠️  No appointments found for this patient")) return;

        var patient = patients.FirstOrDefault(p => p.Id == patientId);

        Console.WriteLine($"\n--- Appointments for Patient: {patient!.Name} ---");
        ConsoleUI.ShowAppointmentsByPatient(appointments, _appointmentService);
    }

    private void ViewAppointmentsByDoctorUI()
    {
        Console.WriteLine("\n--- 👨‍⚕️ View Appointments by Doctor ---");

        var doctors = _appointmentService.GetAllDoctors();
        if (!Validator.IsExist(doctors, "⚠️  No doctors registered")) return;

        Console.WriteLine("\n--- Doctor List ---");
        foreach (var d in doctors)
            Console.WriteLine($"ID: {d.Id} | Name: {d.Name} | Specialty: {d.Specialty}");

        var doctorId = Validator.ValidateGuid("\nEnter Doctor ID: ");

        var appointments = _appointmentService.GetAppointmentsByDoctor(doctorId);
        if (!Validator.IsExist(appointments, "⚠️  No appointments found for this doctor")) return;

        var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);

        Console.WriteLine($"\n--- Appointments for Doctor: {doctor!.Name} ---");
        ConsoleUI.ShowAppointmentsByDoctor(appointments, _appointmentService);
    }

    private void ViewAppointmentsByDateUI()
    {
        Console.WriteLine("\n--- 📅 View Appointments by Date ---");

        string dateInput = Validator.ValidateContent("Enter date (yyyy-MM-dd): ");
        if (!DateTime.TryParse(dateInput, out DateTime date))
        {
            Console.WriteLine("⚠️  Invalid date format");
            return;
        }

        var appointments = _appointmentService.GetAppointmentsByDate(date);
        if (!Validator.IsExist(appointments, "⚠️  No appointments found for this date")) return;

        Console.WriteLine($"\n--- Appointments for Date: {date:yyyy-MM-dd} ---");
        ConsoleUI.ShowAppointmentsByDate(appointments, _appointmentService);
    }

    private void ViewAppointmentsByStatusUI()
    {
        Console.WriteLine("\n--- 📋 View Appointments by Status ---");

        var selectedStatus = Validator.ValidateAppointmentStatus();

        var appointments = _appointmentService.GetAppointmentsByStatus(selectedStatus);
        if (!Validator.IsExist(appointments, $"⚠️  No appointments found with status '{selectedStatus}'")) return;

        Console.WriteLine($"\n--- Appointments with Status: {selectedStatus} ---");
        ConsoleUI.ShowAppointmentsByStatus(appointments, _appointmentService);
    }

}