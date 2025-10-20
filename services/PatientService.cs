namespace SanVicenteHospital.services;

using System;
using System.Collections.Generic;
using SanVicenteHospital.interfaces;
using SanVicenteHospital.models;
using SanVicenteHospital.repositories;
using SanVicenteHospital.utils;

// Service that manages patient-related business logic in the SanVicenteHospital system.
// Handles registration, updates, deletion, and viewing of patients and their patients.
public class PatientService
{
    private readonly IRepository<Patient> _patientRepo;
    private readonly IRepository<Doctor> _doctorRepo;

    public PatientService(IRepository<Patient> patientRepo, IRepository<Doctor> doctorRepo)
    {
        _patientRepo = patientRepo;
        _doctorRepo = doctorRepo;
    }

    // Registers a new patient with optional patients.
    public Patient RegisterPatient(string name, int identification, int age, string address, string phone, string email)
    {
        if (Validator.IsDuplicate(_patientRepo.GetAll(), d => d.Identification, identification, "identification"))
            throw new ArgumentException(" Patient already registered with that identification");
        if (_doctorRepo.GetAll().Any(p => p.Identification == identification))
            throw new ArgumentException(" \nThis identification is already used by a doctor");

        var patient = new Patient(name, identification, age, address, phone, email);

        _patientRepo.Add(patient);
        return patient;
    }

    // Returns all patients.
    public List<Patient> ViewPatients()
    {
        return _patientRepo.GetAll();
    }

    // Updates a patient's data.
    public void UpdatePatient(Guid patientId, string? name = null, int? identification = null, int? age = null, string? address = null, string? phone = null, string? email = null)
    {
        var patient = _patientRepo.GetById(patientId) ?? throw new KeyNotFoundException("Patient not found");

        if (!string.IsNullOrWhiteSpace(name)) patient.Name = name;
        if (identification.HasValue && identification.Value != patient.Identification)
        {
            // Check for duplicates among patients
            if (Validator.IsDuplicate(
                _patientRepo.GetAll().Where(d => d.Id != patientId),
                d => d.Identification,
                identification.Value,
                "identification"))
                throw new ArgumentException("Another patient already has that identification");

            // Check for duplicates among doctors
            if (_doctorRepo.GetAll().Any(d => d.Identification == identification.Value))
                throw new ArgumentException(" \nThis identification is already used by a doctor");

            patient.Identification = identification.Value;
        }
        if (age.HasValue && age.Value > 0) patient.Age = age.Value;
        if (!string.IsNullOrWhiteSpace(address)) patient.Address = address;
        if (!string.IsNullOrWhiteSpace(phone)) patient.Phone = phone;
        if (!string.IsNullOrWhiteSpace(email)) patient.Email = email;

        _patientRepo.Update(patient);
    }

    // Removes a patient and disassociates their patients.
    public void RemovePatient(Guid patientId)
    {
        var patient = _patientRepo.GetById(patientId) ?? throw new KeyNotFoundException("Patient not found");

        _patientRepo.Remove(patientId);
    }

    // Returns a single patient by ID.
    public Patient? GetPatientById(Guid id)
    {
        return _patientRepo.GetById(id);
    }
}

