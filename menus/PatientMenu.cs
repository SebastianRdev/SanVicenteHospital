namespace SanVicenteHospital.menus;

using SanVicenteHospital.models;
using SanVicenteHospital.services;
using SanVicenteHospital.utils;

public class PatientMenu
{
    private readonly PatientService _patientService;

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
                Console.Clear();
                ConsoleUI.ShowPatientMainMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        PatientCRUD();
                        continue;
                    case 2:
                        Console.WriteLine("\nBack to main menu 👥");
                        break;
                    default:
                        Console.WriteLine("\n⚠️ Invalid choice. Please try again");
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

    public void PatientCRUD()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowPatientCRUD();
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
                        RegisterPatientUI();
                        WaitForKey();
                        continue;
                    case 2:
                        ViewPatientsUI();
                        WaitForKey();
                        continue;
                    case 3:
                        UpdatePatientUI();
                        WaitForKey();
                        continue;
                    case 4:
                        RemovePatientUI();
                        WaitForKey();
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to Patient main menu 👥");
                        break;
                    default:
                        Console.WriteLine("\n⚠️ Invalid choice. Please try again");
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

    private void RegisterPatientUI()
    {
        Console.WriteLine("\n--- 📝 Register Patient 👤 ---");
        try
        {
            string name = Validator.ValidateContent("\n👤 Name: ");
            int identification = Validator.ValidatePositiveInt("\n🆔 Identification: ");
            int age = Validator.ValidatePositiveInt("\n🎂 Age: ");
            string address = Validator.ValidateContent("\n🏠 Address: ");
            string phone = Validator.ValidatePhone("\n📞 Phone: ");
            string email = Validator.ValidateEmail("\n✉️  Email: ");

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

        if (!Validator.IsExist(patients, $"⚠️  No patients registered")) return;

        Console.WriteLine("\n--- 👥 Patient List ---");
        ConsoleUI.ShowPatientList(patients);
    }


    private void UpdatePatientUI()
    {
        Console.WriteLine("\n--- ✏️  Update Patient ---");

        try
        {
            // Show available patients
            ViewPatientsUI();

            var patientId = Validator.ValidateGuid("\nEnter Patient ID: ");

            // Find patient
            var patient = _patientService.GetPatientById(patientId);
            if (!Validator.IsExist(patient, "❌ No patient found with that ID")) return;
            if (patient is null) return; // For the compilator

            // Show current data
            ConsoleUI.ShowPatient(patient);

            Console.WriteLine("\n---- Update fields (y/n) ----");

            // Variables with current values
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
                phone = Validator.ValidatePhone("📞 Enter new phone: ");

            if (Validator.AskYesNo("Change email? (y/n): "))
                email = Validator.ValidateEmail("✉️  Enter new email: ");

            _patientService.UpdatePatient(patientId, name, identification, age, address, phone, email);
            Console.WriteLine("\n✅ Patient updated successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("⚠️  Invalid ID format. Please enter a valid value");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("❌ No patient found with that ID");
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

            var patientId = Validator.ValidateGuid("\nEnter Patient ID: ");

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
