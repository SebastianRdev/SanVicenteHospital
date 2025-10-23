namespace SanVicenteHospital.interfaces;

using System;
using System.Collections.Generic;
using SanVicenteHospital.models;
using SanVicenteHospital.models.Enums;

public interface IDoctorService
{
    Doctor RegisterDoctor(string name, int identification, int age, string address, string phone, string email, Specialties specialty);

    List<Doctor> ViewDoctors();

    void UpdateDoctor(Guid doctorId, string? name = null, int? identification = null, int? age = null, string? address = null, string? phone = null, string? email = null, Specialties? specialty = null);

    void RemoveDoctor(Guid doctorId);

    Doctor? GetDoctorById(Guid id);

    List<Doctor> GetDoctorsBySpecialty(Specialties specialty);
}
