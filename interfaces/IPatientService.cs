namespace SanVicenteHospital.interfaces;

using SanVicenteHospital.models;

public interface IPatientService
{
    Patient RegisterPatient(string name, int identification, int age, string address, string phone, string email);

    List<Patient> ViewPatients();

    void UpdatePatient(Guid patientId, string? name = null, int? identification = null, int? age = null, string? address = null, string? phone = null, string? email = null);

    void RemovePatient(Guid patientId);

    Patient? GetPatientById(Guid id);
}
