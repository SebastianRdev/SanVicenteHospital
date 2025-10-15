namespace SanVicenteHospital.services;

using System;
using System.Collections.Generic;
using SanVicenteHospital.interfaces;
using SanVicenteHospital.models;
using SanVicenteHospital.models.Enums;
using SanVicenteHospital.utils;

public class DoctorService
{
    private readonly IRepository<Doctor> _doctorRepo;
    private readonly IRepository<Patient> _patientRepo;

    public DoctorService(IRepository<Doctor> doctorRepo, IRepository<Patient> patientRepo)
    {
        _doctorRepo = doctorRepo;
        _patientRepo = patientRepo;
    }

    // Registers a new doctor.
    public Doctor RegisterDoctor(string name, int identification, int age, string address, string phone, string email, Specialties specialty)
    {
        if (Validator.IsDuplicate(_doctorRepo.GetAll(), d => d.Identification, identification, "identification"))
            throw new ArgumentException(" Doctor already registered with that identification");
        if (_patientRepo.GetAll().Any(p => p.Identification == identification))
            throw new ArgumentException(" \nThis identification is already used by a patient.");
        var doctor = new Doctor(name, identification, age, address, phone, email, specialty);

        _doctorRepo.Add(doctor);
        return doctor;
    }

    // Returns all doctors.
    public List<Doctor> ViewDoctors()
    {
        return _doctorRepo.GetAll();
    }

    // Updates a doctor's data.
    public void UpdateDoctor(Guid doctorId, string? name = null, int? identification = null, int? age = null, string? address = null, string? phone = null, string? email = null, Specialties? specialty = null)
    {
        var doctor = _doctorRepo.GetById(doctorId) ?? throw new KeyNotFoundException("Doctor not found");

        if (!string.IsNullOrWhiteSpace(name)) doctor.Name = name;
        if (identification.HasValue && identification.Value != doctor.Identification)
        {
            // Check for duplicates among doctors
            if (Validator.IsDuplicate(
                _doctorRepo.GetAll().Where(d => d.Id != doctorId),
                d => d.Identification,
                identification.Value,
                "identification"))
                throw new ArgumentException("Another doctor already has that identification");

            // Check for duplicates among patients too
            if (_patientRepo.GetAll().Any(p => p.Identification == identification.Value))
                throw new ArgumentException(" \nThis identification is already used by a patient");

            doctor.Identification = identification.Value;
        }
        if (age.HasValue && age.Value > 0) doctor.Age = age.Value;
        if (!string.IsNullOrWhiteSpace(address)) doctor.Address = address;
        if (!string.IsNullOrWhiteSpace(phone)) doctor.Phone = phone;
        if (!string.IsNullOrWhiteSpace(email)) doctor.Email = email;
        if (specialty.HasValue) doctor.Specialty = specialty.Value;

        _doctorRepo.Update(doctor);
    }

    // Removes a doctor.
    public void RemoveDoctor(Guid doctorId)
    {
        var doctor = _doctorRepo.GetById(doctorId) ?? throw new KeyNotFoundException("Doctor not found");

        _doctorRepo.Remove(doctorId);
    }

    // Returns a single doctor by ID.
    public Doctor? GetDoctorById(Guid id)
    {
        return _doctorRepo.GetById(id);
    }

    // Returns doctors by specialty.
    public List<Doctor> GetDoctorsBySpecialty(Specialties specialty)
    {
        return _doctorRepo.GetAll().Where(d => d.Specialty == specialty).ToList();
    }
}


