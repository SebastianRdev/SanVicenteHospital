namespace SanVicenteHospital.menus;

using SanVicenteHospital.models;
using SanVicenteHospital.services;
using SanVicenteHospital.utils;

public class PatientMenu
{
    private readonly PatientService _patientService;
    bool showPressKey = false;

    public PatientMenu(PatientService patientService)
    {
        _patientService = patientService;
    }

    public void PatientMainMenu()
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
                ConsoleUI.ShowPatientMainMenu();
                Console.Write("\n👉 Enter your choice: ");
                Console.WriteLine($"Es: {showPressKey}");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        PatientCRUD();
                        showPressKey = true;
                        continue;
                    case 2:
                        Console.WriteLine("\nBack to main menu 👥");
                        showPressKey = false;
                        break;
                    default:
                        Console.WriteLine("\n⚠️ Invalid choice. Please try again");
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

    public void PatientCRUD()
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
                ConsoleUI.ShowPatientCRUD();
                Console.Write("\n👉 Enter your choice: ");
                Console.WriteLine($"Es: {showPressKey}");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("\n❌ Invalid input. Please enter a number");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        RegisterPatientUI();
                        showPressKey = true;
                        continue;
                    case 2:
                        ViewPatientsUI();
                        showPressKey = true;
                        continue;
                    case 3:
                        UpdatePatientUI();
                        showPressKey = true;
                        continue;
                    case 4:
                        RemovePatientUI();
                        showPressKey = true;
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to Patient main menu 👥");
                        showPressKey = false;
                        break;
                    default:
                        Console.WriteLine("\n⚠️ Invalid choice. Please try again");
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

    private void RegisterPatientUI()
    {
        Console.WriteLine("\n--- 📝 Register Patient 👤 ---");
        try
        {
            string name = Validator.ValidateContent("\n👤 Name: ");
            int identification = Validator.ValidatePositiveInt("\n🆔 Identification: ");
            int age = Validator.ValidatePositiveInt("\n🎂 Age: ");
            string address = Validator.ValidateContent("\n🏠 Address: ");
            string phone = Validator.ValidateContent("\n📞 Phone: ");
            string email = Validator.ValidateContent("\n✉️  Email: ");

            _patientService.RegisterPatient(name, identification, age, address, phone, email);

            Console.WriteLine("\n✅ Patient registered successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("❌ Invalid input format. Please enter the data correctly.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"⚠️ {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error: {ex.Message}");
        }
    }


    private void ViewPatientsUI()
    {
        var patients = _patientService.ViewPatients().ToList();

        if (patients.Count == 0)
        {
            Console.WriteLine("⚠️  No patients registered");
            return;
        }

        Console.WriteLine("\n--- 👥 Patient List ---");

        foreach (var patient in patients)
        {
            Console.WriteLine($"\n🆔 ID: {patient.Id}");
            Console.WriteLine($"👤 Name: {patient.Name}");
            Console.WriteLine($"👤 Identification: {patient.Identification}");
            Console.WriteLine($"🎂 Age: {patient.Age}");
            Console.WriteLine($"🏠 Address: {patient.Address}");
            Console.WriteLine($"📞 Phone: {patient.Phone}");
            Console.WriteLine($"✉️  Email: {patient.Email}");
        }

        Console.WriteLine($"\n-----------------------");
    }


    private void UpdatePatientUI()
    {
        Console.WriteLine("\n--- ✏️  Update Patient ---");

        try
        {
            // Show available patients
            ViewPatientsUI();

            Console.Write("\nEnter Patient ID: ");
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid patientId))
            {
                Console.WriteLine("⚠️  Invalid ID format");
                return;
            }

            // Find patient
            var patient = _patientService.GetPatientById(patientId);
            if (patient == null)
            {
                Console.WriteLine("❌ No patient found with that ID");
                return;
            }

            // Show current data
            Console.WriteLine($"\nCurrent data for {patient.Name}:");
            Console.WriteLine($"👤 Name: {patient.Name}");
            Console.WriteLine($"👤 Identification: {patient.Identification}");
            Console.WriteLine($"🎂 Age: {patient.Age}");
            Console.WriteLine($"🏠 Address: {patient.Address}");
            Console.WriteLine($"📞 Phone: {patient.Phone}");
            Console.WriteLine($"✉️  Email: {patient.Email}");


            Console.WriteLine("\n---- Update fields (y/n) ----");

            // Variables con valores actuales
            string? name = patient.Name;
            int identification = patient.Identification;
            int age = patient.Age;
            string? address = patient.Address;
            string? phone = patient.Phone;
            string? email = patient.Email;

            // Ask one by one which fields to update
            if (Validator.AskYesNo("Change name? (y/n): "))
                name = Validator.ValidateContent("👤 Enter new name: ");

            if (Validator.AskYesNo("Change age? (y/n): "))
                age = Validator.ValidatePositiveInt("🎂 Enter new age: ");

            if (Validator.AskYesNo("Change identification? (y/n): "))
                identification = Validator.ValidatePositiveInt("👤 Enter new identification: ");

            if (Validator.AskYesNo("Change address? (y/n): "))
                address = Validator.ValidateContent("🏠 Enter new address: ");

            if (Validator.AskYesNo("Change phone? (y/n): "))
                phone = Validator.ValidateContent("📞 Enter new phone: ");

            if (Validator.AskYesNo("Change email? (y/n): "))
                email = Validator.ValidateContent("✉️  Enter new email: ");

            // Llamar al servicio para actualizar
            _patientService.UpdatePatient(patientId, name, identification, age, address, phone, email);
            Console.WriteLine("\n✅ Patient updated successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("⚠️  Invalid ID format. Please enter a valid value");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("❌ No patient found with that ID.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error updating patient: {ex.Message}");
        }
    }



    private void RemovePatientUI()
    {
        Console.WriteLine("\n--- 🗑 Remove Patient ---");
        try
        {
            ViewPatientsUI();

            Console.Write("\nEnter Patient ID: ");
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid patientId))
            {
                Console.WriteLine("⚠️  Invalid ID format");
                return;
            }

            _patientService.RemovePatient(patientId);
            Console.WriteLine("\n✅ Patient removed successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("⚠️ Invalid ID format. Please enter a valid number.");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("❌ No patient found with that ID.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error removing patient: {ex.Message}");
        }
    }
}
