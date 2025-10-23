namespace SanVicenteHospital.menus;

using SanVicenteHospital.models;
using SanVicenteHospital.services;
using SanVicenteHospital.utils;
using SanVicenteHospital.models.Enums;

// The system validates that the identification is unique and that there are no doctors with the same combination of name and specialty
// It should be possible to list all doctors, with an option to filter by specialty.
public class DoctorMenu
{
    private readonly DoctorService _doctorService;

    public DoctorMenu(DoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    public void DoctorMainMenu()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowDoctorMainMenu();
                Console.Write("\n👉 Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        DoctorCRUD();
                        WaitForKey();
                        continue;
                    case 2:
                        ViewDoctorsBySpecialtyUI();
                        WaitForKey();
                        continue;
                    case 3:
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

    public void DoctorCRUD()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                ConsoleUI.ShowDoctorCRUD();
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
                        RegisterDoctorUI();
                        WaitForKey();
                        continue;
                    case 2:
                        ViewDoctorsUI();
                        WaitForKey();
                        continue;
                    case 3:
                        UpdateDoctorUI();
                        WaitForKey();
                        continue;
                    case 4:
                        RemoveDoctorUI();
                        WaitForKey();
                        continue;
                    case 5:
                        Console.WriteLine("\nBack to Doctor main menu 👥");
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

    private void RegisterDoctorUI()
    {
        Console.WriteLine("\n--- 📝 Register Doctor 👤 ---");
        try
        {
            string name = Validator.ValidateContent("\n👤 Name: ");
            int identification = Validator.ValidatePositiveInt("\n🆔 Identification: ");
            int age = Validator.ValidatePositiveInt("\n🎂 Age: ");
            string address = Validator.ValidateContent("\n🏠 Address: ");
            string phone = Validator.ValidatePhone("\n📞 Phone: ");
            string email = Validator.ValidateEmail("\n✉️  Email: ");
            var specialty = Validator.ValidateSpecialty();

            _doctorService.RegisterDoctor(name, identification, age, address, phone, email, specialty);

            Console.WriteLine("\n✅ Doctor registered successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("❌ Invalid input format. Please enter the data correctly");
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


    private void ViewDoctorsUI()
    {
        var doctors = _doctorService.ViewDoctors().ToList();

        if (!Validator.IsExist(doctors, "⚠️  No doctors registered")) return;

        Console.WriteLine("\n--- 👥 Doctor List ---");
        ConsoleUI.ShowDoctorList(doctors);
    }


    private void UpdateDoctorUI()
    {
        Console.WriteLine("\n--- ✏️  Update Doctor ---");

        try
        {
            ViewDoctorsUI();

            var doctorId = Validator.ValidateGuid("\nEnter Doctor ID: ");

            // Search doctor
            var doctor = _doctorService.GetDoctorById(doctorId);
            if (!Validator.IsExist(doctor, "❌ No doctor found with that ID")) return;
            if (doctor is null) return; // For the compilator

            // Current dates
            ConsoleUI.ShowDoctor(doctor);

            Console.WriteLine("\n---- Update fields (y/n) ----");

            // Variables with current values
            string? name = doctor.Name;
            int identification = doctor.Identification;
            int age = doctor.Age;
            string? address = doctor.Address;
            string? phone = doctor.Phone;
            string? email = doctor.Email;
            Specialties specialty = doctor.Specialty;

            // Ask one by one which fields to update
            if (Validator.AskYesNo("Change name? (y/n): "))
                name = Validator.ValidateContent("👤 Enter new name: ");

            if (Validator.AskYesNo("Change identification? (y/n): "))
                identification = Validator.ValidatePositiveInt("👤 Enter new identification: ");

            if (Validator.AskYesNo("Change age? (y/n): "))
                age = Validator.ValidatePositiveInt("🎂 Enter new age: ");

            if (Validator.AskYesNo("Change address? (y/n): "))
                address = Validator.ValidateContent("🏠 Enter new address: ");

            if (Validator.AskYesNo("Change phone? (y/n): "))
                phone = Validator.ValidatePhone("📞 Enter new phone: ");

            if (Validator.AskYesNo("Change email? (y/n): "))
                email = Validator.ValidateEmail("✉️  Enter new email: ");

            if (Validator.AskYesNo("Change specialty? (y/n): "))
            {
                specialty = Validator.ValidateSpecialty();
            }

            _doctorService.UpdateDoctor(doctorId, name, identification, age, address, phone, email, specialty);
            Console.WriteLine("\n✅ Doctor updated successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("⚠️  Invalid ID format. Please enter a valid value");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("❌ No doctor found with that ID.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error updating doctor: {ex.Message}");
        }
    }



    private void RemoveDoctorUI()
    {
        Console.WriteLine("\n--- 🗑 Remove Doctor ---");
        try
        {
            ViewDoctorsUI();

            var doctorId = Validator.ValidateGuid("\nEnter Doctor ID: ");

            _doctorService.RemoveDoctor(doctorId);
            Console.WriteLine("\n✅ Doctor removed successfully!");
        }
        catch (FormatException)
        {
            Console.WriteLine("⚠️  Invalid ID format. Please enter a valid number");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("❌ No doctor found with that ID.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error removing doctor: {ex.Message}");
        }
    }

    private void ViewDoctorsBySpecialtyUI()
    {
        Console.WriteLine("\n--- 🔍 View Doctors by Specialty ---");

        var specialty = Validator.ValidateSpecialty();

        var doctors = _doctorService.GetDoctorsBySpecialty(specialty).ToList();

        if (!Validator.IsExist(doctors, $"⚠️  No doctors found with specialty {specialty}")) return;

        Console.WriteLine($"\n--- 👥 Doctors with specialty {specialty} ---");
        ConsoleUI.ShowDoctorList(doctors);
    }
}
