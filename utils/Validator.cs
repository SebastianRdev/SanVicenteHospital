namespace SanVicenteHospital.utils;

using System.Text.RegularExpressions;
using SanVicenteHospital.models.Enums;

// Utility class for common validations in the SanVicenteHospital system.
public static class Validator
{
    // Requests and validates that the entered content is not empty.
    public static string ValidateContent(string prompt)
    {
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine()!;
            if (IsEmpty(input))
                return input;
        }
    }

    // Requests and validates that the entered number is positive.
    public static int ValidatePositiveInt(string prompt)
    {
        int value;
        while (true)
        {
            try
            {
                Console.Write(prompt);
                value = Convert.ToInt32(Console.ReadLine());
                if (IsPositive(value))
                    return value;
            }
            catch
            {
                Console.WriteLine("❌ Invalid input. Please enter a number");
            }
        }
    }

    public static string ValidateEmail(string prompt)
    {
        string email;
        while (true)
        {
            Console.Write(prompt);
            email = Console.ReadLine()!;
            if (IsValidEmail(email))
                return email;
        }
    }

    public static string ValidatePhone(string prompt)
    {
        string phone;
        while (true)
        {
            Console.Write(prompt);
            phone = Console.ReadLine()!;
            if (IsValidPhone(phone))
                return phone;
        }
    }

    // Requests and validates that the entered content is not empty, with an option to allow empty input.
    public static string ValidateContentEmpty(string prompt, bool allowEmpty = false)
    {
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine()!;

            if (allowEmpty || !string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("⚠️  Empty spaces are not allowed");
        }
    }

    //Check if an integer is positive.

    public static bool IsPositive(int number)
    {
        if (number < 0)
        {
            Console.WriteLine("⚠️  Please enter positive integers");
            return false;
        }

        return true;
    }

    // Checks whether a text string is not empty or consists only of spaces.
    public static bool IsEmpty(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("⚠️  Empty spaces are not allowed");
            return false;
        }

        return true;
    }

    // Checks whether a list or object exists (is not null or empty) and displays a message if it does not.
    public static bool IsExist<T>(List<T>? list, string message)
    {
        if (list == null || list.Count == 0)
        {
            Console.WriteLine($"{message}");
            return false;
        }
        return true;
    }

    // Checks whether an object exists (is not null) and displays a message if it does not.
    public static bool IsExist<T>(T? obj, string message) where T : class
    {
        if (obj == null)
        {
            Console.WriteLine($"{message}");
            return false;
        }
        return true;
    }

    public static bool AskYesNo(string message)
    {
        while (true)
        {
            Console.Write(message);
            var response = Console.ReadLine()?.Trim().ToLower();

            if (response is "y" or "yes" or "s" or "si")
                return true;
            if (response is "n" or "no")
                return false;

            Console.WriteLine("⚠️  Please enter 'y' (yes) or 'n' (no)");
        }
    }

    // Checks whether a value already exists in a collection, based on a selector function.
    public static bool IsDuplicate<T, TKey>(IEnumerable<T> collection, Func<T, TKey> selector, TKey value, string fieldName)
    {
        if (collection.Any(item => EqualityComparer<TKey>.Default.Equals(selector(item), value)))
        {
            Console.WriteLine($"⚠️  A record with the same {fieldName} already exists.");
            return true;
        }

        return false;
    }


    public static bool IsValidEmail(string email)
    {
        if (IsEmpty(email))
            return false;

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(email, pattern))
        {
            Console.WriteLine("⚠️  Invalid email format. Example: user@mail.com");
            return false;
        }

        return true;
    }

    public static bool IsValidPhone(string phone)
    {
        // Allow numbers with or without + at start, between 7 and 15 digits
        string pattern = @"^\+?\d{7,15}$";

        if (!Regex.IsMatch(phone, pattern))
        {
            Console.WriteLine("⚠️  Invalid phone number. Use only digits (optionally with + at the start, e.g., +573001234567)");
            return false;
        }

        return true;
    }

    public static Guid ValidateGuid(string message)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        if (!Guid.TryParse(input, out Guid id))
            throw new FormatException("⚠️ Invalid ID format.");

        return id;
    }
    public static Specialties ValidateSpecialty()
    {
        Console.WriteLine("\n🧼 --- Specialties ---");
        foreach (var s in Enum.GetValues(typeof(Specialties)))
            Console.WriteLine($"{(int)s}. {s}");

        int specialtyInt = ValidatePositiveInt("\nEnter specialty (number): ");

        if (!Enum.IsDefined(typeof(Specialties), specialtyInt))
            throw new ArgumentException("⚠️  Invalid specialty number");

        return (Specialties)specialtyInt;
    }

    public static ServiceType ValidateServiceType()
    {
        Console.WriteLine("\n--- Available Services ---");
        foreach (var s in Enum.GetValues(typeof(ServiceType)))
            Console.WriteLine($"{(int)s}. {s}");

        int serviceInt = ValidatePositiveInt("\nEnter service (number): ");

        if (!Enum.IsDefined(typeof(ServiceType), serviceInt))
            throw new ArgumentException("⚠️  Invalid service number");

        return (ServiceType)serviceInt;
    }

    public static AppointmentStatus ValidateAppointmentStatus()
    {
        Console.WriteLine("\n--- Available Statuses ---");
        foreach (var status in Enum.GetValues(typeof(AppointmentStatus)))
            Console.WriteLine($"{(int)status}. {status}");

        int statusInt = ValidatePositiveInt("\nEnter service (number): ");

        if (!Enum.IsDefined(typeof(AppointmentStatus), statusInt))
            throw new ArgumentException("⚠️  Invalid status number");

        return (AppointmentStatus)statusInt;
    }

    
}
