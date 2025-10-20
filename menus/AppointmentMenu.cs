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
            if (patients.Count == 0)
            {
                Console.WriteLine("⚠️  No patients registered. Please register a patient first");
                return;
            }

            var vets = _appointmentService.GetAllDoctors();
            if (vets.Count == 0)
            {
                Console.WriteLine("⚠️  No doctors registered. Please register one first");
                return;
            }

                    // Show available patients
            Console.WriteLine("\n --- Patient List ---");
            foreach (var p in patients)
                Console.WriteLine($"ID: {p.Id} | Name: {p.Name}");

            string patientInput = Validator.ValidateContent("\nEnter Patient ID: ");
            if (!Guid.TryParse(patientInput, out Guid patientId))
            {
                Console.WriteLine("⚠️  Invalid Patient ID format");
                return;
            }

            // 🩺 Show available doctors
            Console.WriteLine("\n👨‍⚕️ --- Doctor List ---");
            foreach (var v in vets)
                Console.WriteLine($"ID: {v.Id} | Name: {v.Name} | Specialty: {v.Specialty}");

            string doctorInput = Validator.ValidateContent("\nEnter Doctor ID: ");
            if (!Guid.TryParse(doctorInput, out Guid doctorId))
            {
                Console.WriteLine("⚠️  Invalid Doctor ID format");
                return;
            }

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
            Console.WriteLine("\n💉 --- Available Services ---");
            foreach (var s in Enum.GetValues(typeof(ServiceType)))
                Console.WriteLine($"{(int)s}. {s}");

            int serviceInt = Validator.ValidatePositiveInt("\nEnter service number: ");
            if (!Enum.IsDefined(typeof(ServiceType), serviceInt))
            {
                Console.WriteLine("⚠️  Invalid service number");
                return;
            }
            var serviceType = (ServiceType)serviceInt;

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
        if (appointments.Count == 0)
        {
            Console.WriteLine("⚠️  No appointments registered");
            return;
        }

        Console.WriteLine("\n--- 📅  View Appointments ---");

        foreach (var a in appointments)
        {
            var patient = _appointmentService.GetAllPatients().FirstOrDefault(p => p.Id == a.PatientId);
            var doctor = _appointmentService.GetAllDoctors().FirstOrDefault(d => d.Id == a.DoctorId);
            Console.WriteLine($"\n🆔 {a.Id}");
            Console.WriteLine($"🧍 Patient: {patient?.Name ?? "Unknown"} ({a.PatientId})");
            Console.WriteLine($"🩺 Doctor: {doctor?.Name ?? "Unknown"} ({a.DoctorId})");
            Console.WriteLine($"📅 Start Time: {a.StartTime}");
            Console.WriteLine($"📅 End Time: {a.EndTime}");
            Console.WriteLine($"🧼 Service: {a.ServiceType}");
            Console.WriteLine($"🗒️  Reason: {a.Reason}");
            Console.WriteLine($"📋 Status: {a.Status}");
        }

        Console.WriteLine("\n--- End of Appointments List ---");
    }

    private void UpdateAppointmentUI()
    {
        Console.WriteLine("\n--- ✏️  Update Appointment ---");

        try
        {
            var appointments = _appointmentService.ViewAppointments().OrderBy(a => a.StartTime).ToList();
            if (appointments.Count == 0)
            {
                Console.WriteLine("⚠️  No appointments registered");
                return;
            }

            foreach (var a in appointments)
            {
                Console.WriteLine($"\n🆔 {a.Id} | 🩺 {a.ServiceType} | 📅 {a.StartTime} : {a.EndTime} | 📋 Status: {a.Status}");
            }

            string idInput = Validator.ValidateContent("\nEnter Appointment ID: ");
            if (!Guid.TryParse(idInput, out Guid appointmentId))
            {
                Console.WriteLine("⚠️  Invalid ID format");
                return;
            }

            // Get current appointment
            var appointment = _appointmentService.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                Console.WriteLine("❌ No appointment found with that ID");
                return;
            }

            Console.WriteLine($"\nCurrent appointment details:");
            Console.WriteLine($"🐕 Patient ID: {appointment.PatientId}");
            Console.WriteLine($"👨‍⚕️ Doctor ID: {appointment.DoctorId}");
            Console.WriteLine($"📅 Start Time: {appointment.StartTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"📅 End Time: {appointment.EndTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"💉 Service: {appointment.ServiceType}");
            Console.WriteLine($"📝 Reason: {appointment.Reason}");

            Console.WriteLine("\nUpdate fields (y/n):");

            Guid? newPatientId = appointment.PatientId;
            if (Validator.AskYesNo("Change patient? (y/n): "))
            {
                var patients = _appointmentService.GetAllPatients();
                if (patients.Count == 0)
                {
                    Console.WriteLine("⚠️  No patients available. Please register one first");
                    return;
                }

                Console.WriteLine("\n--- Patient List ---");
                foreach (var patient in patients)
                    Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name}");

                string patientInput = Validator.ValidateContent("Enter new Patient ID: ");
                if (Guid.TryParse(patientInput, out Guid patientId))
                    newPatientId = patientId;
                else
                    Console.WriteLine("⚠️  Invalid Patient ID format. Patient not changed");
            }

            Guid? newVetId = appointment.DoctorId;
            if (Validator.AskYesNo("Change doctor? (y/n): "))
            {
                var vets = _appointmentService.GetAllDoctors();
                if (vets.Count == 0)
                {
                    Console.WriteLine("⚠️  No doctors available. Please register one first");
                    return;
                }

                Console.WriteLine("\n--- Doctor List ---");
                foreach (var v in vets)
                    Console.WriteLine($"ID: {v.Id} | Name: {v.Name} | Specialty: {v.Specialty}");

                string vetInput = Validator.ValidateContent("Enter new Doctor ID: ");
                if (Guid.TryParse(vetInput, out Guid vetId))
                    newVetId = vetId;
                else
                    Console.WriteLine("⚠️  Invalid Doctor ID format. Doctor not changed");
            }

            DateTime newStartTime = appointment.StartTime;
            if (Validator.AskYesNo("Change start time? (y/n): "))
            {
                string startTimeInput = Validator.ValidateContent("Enter new start time (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(startTimeInput, out DateTime startTime))
                    newStartTime = startTime;
            }

            DateTime newEndTime = appointment.EndTime;
            if (Validator.AskYesNo("Change end time? (y/n): "))
            {
                string endTimeInput = Validator.ValidateContent("Enter new end time (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(endTimeInput, out DateTime endTime))
                    newEndTime = endTime;
                else
                    Console.WriteLine("⚠️  Invalid date format. Date not changed");
            }

            ServiceType newService = appointment.ServiceType;
            if (Validator.AskYesNo("Change service type? (y/n): "))
            {
                Console.WriteLine("\n--- Available Services ---");
                foreach (var s in Enum.GetValues(typeof(ServiceType)))
                    Console.WriteLine($"{(int)s}. {s}");

                int serviceInt = Validator.ValidatePositiveInt("Select service number: ");
                if (Enum.IsDefined(typeof(ServiceType), serviceInt))
                    newService = (ServiceType)serviceInt;
                else
                    Console.WriteLine("⚠️  Invalid service number. Service not changed");
            }

            string newReason = appointment.Reason;
            if (Validator.AskYesNo("Change reason? (y/n): "))
                newReason = Validator.ValidateContent("📝 Enter new reason: ");

            _appointmentService.UpdateAppointment(
                appointmentId,
                newPatientId,
                newVetId,
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
        if (appointments.Count == 0)
        {
            Console.WriteLine("⚠️  No appointments registered");
            return;
        }
        foreach (var a in appointments)
        {
            Console.WriteLine($"\n🆔 {a.Id} | 🩺 {a.ServiceType} | 📅 {a.StartTime} : {a.EndTime} | 📋 Status: {a.Status}");
        }

        Console.Write("\nEnter Appointment ID to remove: ");
        var appointmentIdInput = Console.ReadLine();

        if (!Guid.TryParse(appointmentIdInput, out Guid appointmentId))
        {
            Console.WriteLine("⚠️  Invalid ID format");
            return;
        }

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
        if (appointments.Count == 0)
        {
            Console.WriteLine("⚠️  No appointments registered");
            return;
        }

        foreach (var a in appointments)
        {
            Console.WriteLine($"\n🆔 {a.Id}");
            Console.WriteLine($"📅 Start Time: {a.StartTime}");
            Console.WriteLine($"📅 End Time: {a.EndTime}");
            Console.WriteLine($"💉 Service: {a.ServiceType}");
            Console.WriteLine($"📋 Status: {a.Status}");
            Console.WriteLine("-----------------------");
        }

        string idInput = Validator.ValidateContent("\nEnter Appointment ID to change status: ");
        if (!Guid.TryParse(idInput, out Guid appointmentId))
        {
            Console.WriteLine("⚠️  Invalid ID format");
            return;
        }

        var appointment = _appointmentService.GetAppointmentById(appointmentId);
        if (appointment == null)
        {
            Console.WriteLine("❌ Appointment not found");
            return;
        }

        Console.WriteLine($"\nCurrent status: {appointment.Status}");
        Console.WriteLine("\n--- Available Statuses ---");
        foreach (var status in Enum.GetValues(typeof(AppointmentStatus)))
            Console.WriteLine($"{(int)status}. {status}");

        int statusInt = Validator.ValidatePositiveInt("\nEnter new status number: ");
        if (!Enum.IsDefined(typeof(AppointmentStatus), statusInt))
        {
            Console.WriteLine("⚠️  Invalid status number");
            return;
        }

        var newStatus = (AppointmentStatus)statusInt;

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
        if (patients.Count == 0)
        {
            Console.WriteLine("⚠️  No patients registered");
            return;
        }

        Console.WriteLine("\n--- Patient List ---");
        foreach (var p in patients)
            Console.WriteLine($"ID: {p.Id} | Name: {p.Name}");

        string input = Validator.ValidateContent("\nEnter Patient ID: ");
        if (!Guid.TryParse(input, out Guid patientId))
        {
            Console.WriteLine("⚠️  Invalid ID format");
            return;
        }

        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        if (patient == null)
        {
            Console.WriteLine("⚠️  Patient not found");
            return;
        }

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (appointments.Count == 0)
        {
            Console.WriteLine("⚠️  No appointments found for this patient");
            return;
        }

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
        if (doctors.Count == 0)
        {
            Console.WriteLine("⚠️  No doctors registered");
            return;
        }

        Console.WriteLine("\n--- Doctor List ---");
        foreach (var d in doctors)
            Console.WriteLine($"ID: {d.Id} | Name: {d.Name} | Specialty: {d.Specialty}");

        string input = Validator.ValidateContent("\nEnter Doctor ID: ");
        if (!Guid.TryParse(input, out Guid doctorId))
        {
            Console.WriteLine("⚠️  Invalid ID format");
            return;
        }

        var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);
        if (doctor == null)
        {
            Console.WriteLine("⚠️  Doctor not found");
            return;
        }

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (appointments.Count == 0)
        {
            Console.WriteLine("⚠️  No appointments found for this doctor");
            return;
        }

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

        if (appointments.Count == 0)
        {
            Console.WriteLine("⚠️  No appointments found for this date");
            return;
        }

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

        Console.WriteLine("\n--- Available Statuses ---");
        foreach (var status in Enum.GetValues(typeof(AppointmentStatus)))
            Console.WriteLine($"{(int)status}. {status}");

        int statusInt = Validator.ValidatePositiveInt("\nEnter status number: ");
        if (!Enum.IsDefined(typeof(AppointmentStatus), statusInt))
        {
            Console.WriteLine("⚠️  Invalid status number");
            return;
        }

        var selectedStatus = (AppointmentStatus)statusInt;

        var appointments = _appointmentService.ViewAppointments()
            .Where(a => a.Status == selectedStatus)
            .OrderBy(a => a.StartTime)
            .ToList();

        if (appointments.Count == 0)
        {
            Console.WriteLine($"⚠️  No appointments found with status '{selectedStatus}'");
            return;
        }

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