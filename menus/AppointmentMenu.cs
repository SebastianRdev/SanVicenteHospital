namespace SanVicenteHospital.menus;

using SanVicenteHospital.utils;
using SanVicenteHospital.services;
using SanVicenteHospital.models.Enums;

public class AppointmentMenu
{
    private readonly AppointmentService _appointmentService;
    private readonly EmailService _emailService;
    bool showPressKey = false;

    public AppointmentMenu(AppointmentService appointmentService, EmailService emailService)
    {
        _appointmentService = appointmentService;
        _emailService = emailService;
    }

    public void AppointmentMainMenu()
    {
        while (true)
        {
            try
            {
                if (showPressKey)
                {
                    Console.WriteLine("\nPress any key to display the menu...");
                    Console.ReadKey();
                }
                Console.Clear();
                ConsoleUI.ShowAppointmentsMainMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AppointmentsCRUD();
                        showPressKey = true;
                        continue;
                    case 2:
                        ChangeAppointmentStatusUI();
                        showPressKey = true;
                        continue;
                    case 3:
                        ViewAppointmentsFilteredByMenuUI();
                        showPressKey = true;
                        continue;
                    case 4:
                        _emailService.ViewEmailHistory();
                        showPressKey = true;
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to main menu");
                        showPressKey = false;
                        break;
                    default:
                        Console.WriteLine("\n⚠️  Invalid choice. Please try again");
                        continue;
                }
            }
            catch
            {
                Console.WriteLine("\n❌ Invalid input. Please enter a number");
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
                if (showPressKey)
                {
                    Console.WriteLine("\nPress any key to display the menu...");
                    Console.ReadKey();
                }
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
                        showPressKey = true;
                        continue;
                    case 2:
                        ViewAppointmentsUI();
                        showPressKey = true;
                        continue;
                    case 3:
                        UpdateAppointmentUI();
                        showPressKey = true;
                        continue;
                    case 4:
                        RemoveAppointmentUI();
                        showPressKey = true;
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to main menu");
                        showPressKey = false;
                        break;
                    default:
                        Console.WriteLine("\n⚠️  Invalid choice. Please try again");
                        continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ {ex.Message}");
                continue;
            }
            break;
        }
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
            if (appointment is null) return; // For the compilator

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

                var patientId = Validator.ValidateGuid("\nEnter Patient ID: ");
            }
            
            if (Validator.AskYesNo("Change doctor? (y/n): "))
            {
                var doctors = _appointmentService.GetAllDoctors();
                if (!Validator.IsExist(doctors, "⚠️  No doctors available. Please register one first")) return;

                Console.WriteLine("\n--- Doctor List ---");
                foreach (var d in doctors)
                    Console.WriteLine($"ID: {d.Id} | Name: {d.Name} | Specialty: {d.Specialty}");

                var doctorId = Validator.ValidateGuid("\nEnter Doctor ID: ");
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
        if (appointment is null) return; // For the compilator

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
                ConsoleUI.ShowAppointmentsByMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ViewAppointmentsByPatientUI();
                        continue;
                    case 2:
                        ViewAppointmentsByDoctorUI();
                        continue;
                    case 3:
                        ViewAppointmentsByDateUI();
                        continue;
                    case 4:
                        ViewAppointmentsByStatusUI();
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

        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        if (!Validator.IsExist(patient, "⚠️  Patient not found")) return;
        if (patient is null) return; // For the compilator

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (!Validator.IsExist(appointments, "⚠️  No appointments found for this patient")) return;

        Console.WriteLine($"\n--- Appointments for Patient: {patient.Name} ---");

        foreach (var a in appointments)
        {
            var doctor = _appointmentService.GetAllDoctors().FirstOrDefault(d => d.Id == a.DoctorId);
            Console.WriteLine($"\n🗓️  {a.StartTime} : {a.EndTime}");
            Console.WriteLine($"💉 Service: {a.ServiceType}");
            Console.WriteLine($"🩺 Doctor: {doctor?.Name ?? "Unknown"} ({a.DoctorId})");
            Console.WriteLine($"📋 Status: {a.Status}");
        }

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

        var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);
        if (!Validator.IsExist(doctor, "⚠️  Doctor not found")) return;
        if (doctor is null) return; // For the compilator

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (!Validator.IsExist(appointments, "⚠️  No appointments found for this doctor")) return;

        Console.WriteLine($"\n--- Appointments for Doctor: {doctor.Name} ---");

        foreach (var a in appointments)
        {
            var patient = _appointmentService.GetAllPatients().FirstOrDefault(p => p.Id == a.PatientId);
            Console.WriteLine($"\n🗓️  {a.StartTime} : {a.EndTime}");
            Console.WriteLine($"💉 Service: {a.ServiceType}");
            Console.WriteLine($"🧍 Patient: {patient?.Name ?? "Unknown"} ({a.PatientId})");
            Console.WriteLine($"📋 Status: {a.Status}");
        }

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

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.StartTime.Date == date.Date)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (!Validator.IsExist(appointments, "⚠️  No appointments found for this date")) return;

        Console.WriteLine($"\n--- Appointments for Date: {date:yyyy-MM-dd} ---");

        foreach (var a in appointments)
        {
            var patient = _appointmentService.GetAllPatients().FirstOrDefault(p => p.Id == a.PatientId);
            var doctor = _appointmentService.GetAllDoctors().FirstOrDefault(d => d.Id == a.DoctorId);

            Console.WriteLine($"\n🕓 {a.StartTime:HH:mm} - {a.EndTime:HH:mm}");
            Console.WriteLine($"💉 {a.ServiceType}");
            Console.WriteLine($"🧍 Patient: {patient?.Name ?? "Unknown"} ({a.PatientId})");
            Console.WriteLine($"🩺 Doctor: {doctor?.Name ?? "Unknown"} ({a.DoctorId})");
            Console.WriteLine($"📋 Status: {a.Status}");
        }

    }

    private void ViewAppointmentsByStatusUI()
    {
        Console.WriteLine("\n--- 📋 View Appointments by Status ---");

        var selectedStatus = Validator.ValidateAppointmentStatus();

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.Status == selectedStatus)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (!Validator.IsExist(appointments, $"⚠️  No appointments found with status '{selectedStatus}'")) return;

        Console.WriteLine($"\n--- Appointments with Status: {selectedStatus} ---");

        foreach (var a in appointments)
        {
            var patient = _appointmentService.GetAllPatients().FirstOrDefault(p => p.Id == a.PatientId);
            var doctor = _appointmentService.GetAllDoctors().FirstOrDefault(d => d.Id == a.DoctorId);

            Console.WriteLine($"\n🗓️  {a.StartTime} - {a.EndTime}");
            Console.WriteLine($"💉 {a.ServiceType}");
            Console.WriteLine($"🧍 Patient: {patient?.Name ?? "Unknown"} ({a.PatientId})");
            Console.WriteLine($"🩺 Doctor: {doctor?.Name ?? "Unknown"} ({a.DoctorId})");
        }
    }

}