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
                        continue;
                    case 2:
                        ViewDoctorsBySpecialtyUI();
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
                        continue;
                    case 2:
                        ViewDoctorsUI();
                        continue;
                    case 3:
                        UpdateDoctorUI();
                        continue;
                    case 4:
                        RemoveDoctorUI();
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
                continue;
            }
            break;
        }
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
            string phone = Validator.ValidateContent("\n📞 Phone: ");
            string email = Validator.ValidateContent("\n✉️  Email: ");

            Console.WriteLine("\n🧼 --- Specialties ---");
            foreach (var specialty in Enum.GetValues(typeof(Specialties)))
                Console.WriteLine($"{(int)specialty}. {specialty}");

            int specialtyInt = Validator.ValidatePositiveInt("\nEnter specialty (number): ");
            if (!Enum.IsDefined(typeof(Specialties), specialtyInt))
            {
                Console.WriteLine("⚠️  Invalid specialty number");
                return;
            }
            var specialtyType = (Specialties)specialtyInt;

            _doctorService.RegisterDoctor(name, identification, age, address, phone, email, specialtyType);

            Console.WriteLine("\n✅ Doctor registered successfully!");
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


    private void ViewDoctorsUI()
    {
        var doctors = _doctorService.ViewDoctors().ToList();

        if (doctors.Count == 0)
        {
            Console.WriteLine("⚠️  No doctors registered");
            return;
        }

        Console.WriteLine("\n--- 👥 Doctor List ---");

        foreach (var doctor in doctors)
        {
            Console.WriteLine($"\n🆔 ID: {doctor.Id}");
            Console.WriteLine($"👤 Name: {doctor.Name}");
            Console.WriteLine($"👤 Identification: {doctor.Identification}");
            Console.WriteLine($"🎂 Age: {doctor.Age}");
            Console.WriteLine($"🏠 Address: {doctor.Address}");
            Console.WriteLine($"📞 Phone: {doctor.Phone}");
            Console.WriteLine($"✉️  Email: {doctor.Email}");
            Console.WriteLine($"🩺 Specialty: {doctor.Specialty}");
        }

        Console.WriteLine($"\n-----------------------");
    }


    private void UpdateDoctorUI()
    {
        Console.WriteLine("\n--- ✏️  Update Doctor ---");

        try
        {
            // Mostrar doctores disponibles
            ViewDoctorsUI();

            Console.Write("\nEnter Doctor ID: ");
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid doctorId))
            {
                Console.WriteLine("⚠️  Invalid ID format");
                return;
            }

            // Buscar doctor
            var doctor = _doctorService.GetDoctorById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("❌ No doctor found with that ID");
                return;
            }

            // Mostrar datos actuales
            Console.WriteLine($"\nCurrent data for {doctor.Name}:");
            Console.WriteLine($"👤 Name: {doctor.Name}");
            Console.WriteLine($"👤 Identification: {doctor.Identification}");
            Console.WriteLine($"🎂 Age: {doctor.Age}");
            Console.WriteLine($"🏠 Address: {doctor.Address}");
            Console.WriteLine($"📞 Phone: {doctor.Phone}");
            Console.WriteLine($"✉️  Email: {doctor.Email}");
            Console.WriteLine($"🩺 Specialty: {doctor.Specialty}");


            Console.WriteLine("\n---- Update fields (y/n) ----");

            // Variables con valores actuales
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
                phone = Validator.ValidateContent("📞 Enter new phone: ");

            if (Validator.AskYesNo("Change email? (y/n): "))
                email = Validator.ValidateContent("✉️  Enter new email: ");

            if (Validator.AskYesNo("Change specialty? (y/n): "))
            {
                Console.WriteLine("\n🧼 --- Specialties ---");
                foreach (var s in Enum.GetValues(typeof(Specialties)))
                    Console.WriteLine($"{(int)s}. {s}");

                int specialtyInt = Validator.ValidatePositiveInt("\nSelect specialty number: ");
                if (Enum.IsDefined(typeof(Specialties), specialtyInt))
                    specialty = (Specialties)specialtyInt;
                else
                {
                    Console.WriteLine("⚠️  Invalid specialty number. Specialty not changed");
                }
            }

            // Llamar al servicio para actualizar
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

            Console.Write("\nEnter Doctor ID: ");
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid doctorId))
            {
                Console.WriteLine("⚠️  Invalid ID format");
                return;
            }

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

        Console.WriteLine("\n🧼 --- Specialties ---");
        foreach (var specialty in Enum.GetValues(typeof(Specialties)))
            Console.WriteLine($"{(int)specialty}. {specialty}");

        int specialtyInt = Validator.ValidatePositiveInt("\nSelect specialty number: ");
        if (!Enum.IsDefined(typeof(Specialties), specialtyInt))
        {
            Console.WriteLine("⚠️  Invalid specialty number");
            return;
        }
        var specialtyType = (Specialties)specialtyInt;

        var doctors = _doctorService.GetDoctorsBySpecialty(specialtyType).ToList();

        if (doctors.Count == 0)
        {
            Console.WriteLine($"⚠️  No doctors found with specialty {specialtyType}");
            return;
        }

        Console.WriteLine($"\n--- 👥 Doctors with specialty {specialtyType} ---");

        foreach (var doctor in doctors)
        {
            Console.WriteLine($"\n🆔 ID: {doctor.Id}");
            Console.WriteLine($"👤 Name: {doctor.Name}");
            Console.WriteLine($"👤 Identification: {doctor.Identification}");
            Console.WriteLine($"🎂 Age: {doctor.Age}");
            Console.WriteLine($"🏠 Address: {doctor.Address}");
            Console.WriteLine($"📞 Phone: {doctor.Phone}");
            Console.WriteLine($"✉️  Email: {doctor.Email}");
            Console.WriteLine($"🩺 Specialty: {doctor.Specialty}");
        }

        Console.WriteLine($"\n-----------------------");
    }
}
