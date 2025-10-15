namespace SanVicenteHospital.models;

using SanVicenteHospital.interfaces;

public class Patient : Person, IEntity
{

    public Patient(string name, int identification, int age, string address, string phone, string email)
    {
        Name = name;
        Identification = identification;
        Age = age;
        Address = address;
        Phone = phone;
        Email = email;
    }
}