using SanVicenteHospital.models;
using SanVicenteHospital.models.Enums;
using SanVicenteHospital.services;

namespace SanVicenteHospital.seeders;

public static class DatabaseSeeder
{
    public static void Seed(PatientService patientService, DoctorService doctorService, AppointmentService appointmentService)
    {
        // 👩‍⚕️ Pacientes
        var patient1 = patientService.RegisterPatient("Juan", 12345, 30, "Calle 10", "555-1234", "juan@mail.com");
        var patient2 = patientService.RegisterPatient("Ana", 67890, 28, "Avenida 5", "555-5678", "ana@mail.com");

        // 🧑‍⚕️ Doctores
        var doctor1 = doctorService.RegisterDoctor("Dra. María Pérez", 555, 40, "Av. Central 15", "3123456789", "maria@hospital.com", Specialties.Cardiology);
        var doctor2 = doctorService.RegisterDoctor("Dr. Juan López", 777, 45, "Calle 8 #9", "3105557777", "juan@hospital.com", Specialties.Dermatology);
        var doctor3 = doctorService.RegisterDoctor("Dr. Carlos Gómez", 888, 38, "Carrera 12 #45", "3119998888", "carlos@hospital.com", Specialties.Neurology);

        // 📅 Citas
        appointmentService.RegisterAppointment(
            patient1.Id,
            doctor1.Id,
            DateTime.Now.AddDays(2).AddHours(9),
            DateTime.Now.AddDays(2).AddHours(10),
            ServiceType.GeneralConsultation,
            "General checkup"
        );

        appointmentService.RegisterAppointment(
            patient2.Id,
            doctor2.Id,
            DateTime.Now.AddDays(5).AddHours(11),
            DateTime.Now.AddDays(5).AddHours(12),
            ServiceType.Surgery,
            "Skin checkup"
        );

        appointmentService.RegisterAppointment(
            patient1.Id,
            doctor3.Id,
            DateTime.Now.AddDays(1).AddHours(14),
            DateTime.Now.AddDays(1).AddHours(15),
            ServiceType.NeurologyConsultation,
            "Frequent headaches"
        );

        appointmentService.RegisterAppointment(
            patient2.Id,
            doctor1.Id,
            DateTime.Now.AddDays(3).AddHours(10),
            DateTime.Now.AddDays(3).AddHours(11),
            ServiceType.CardiologyConsultation,
            "Postoperative checkup"
        );

        appointmentService.RegisterAppointment(
            patient1.Id,
            doctor2.Id,
            DateTime.Now.AddDays(7).AddHours(16),
            DateTime.Now.AddDays(7).AddHours(17),
            ServiceType.DermatologyConsultation,
            "Skin checkup"
        );

        Console.WriteLine("\n✅ Database seeded successfully with sample data.\n");
    }
}
