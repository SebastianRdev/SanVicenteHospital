namespace SanVicenteHospital.models;

using SanVicenteHospital.interfaces;
using SanVicenteHospital.models.Enums;

public class Doctor : Person, IEntity
{
    public Specialties Specialty { get; set; }

    public bool IsActive { get; set; } = true;

    // Creates a new Doctor instance with the provided information.
    public Doctor(string name, int identification,int age, string address, string phone, string email, Specialties specialty)
    {
        Name = name;
        Identification = identification;
        Age = age;
        Address = address;
        Phone = phone;
        Email = email;
        Specialty = specialty;
        IsActive = true;
    }
}
