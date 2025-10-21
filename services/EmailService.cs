namespace SanVicenteHospital.services;

using System.Net;
using System.Net.Mail;
using SanVicenteHospital.models;
using SanVicenteHospital.repositories;
using SanVicenteHospital.interfaces;
using DotNetEnv;

public class EmailService
{
    private readonly IRepository<EmailLog> _emailRepo;
    private readonly string _emailUser;
    private readonly string _emailPass;
    public EmailService(IRepository<EmailLog> emailRepo)
    {
        _emailRepo = emailRepo;

        _emailUser = Environment.GetEnvironmentVariable("EMAIL_USER") ?? throw new Exception("EMAIL_USER not found");
        _emailPass = Environment.GetEnvironmentVariable("EMAIL_PASS") ?? throw new Exception("EMAIL_PASS not found");
    }

    public EmailLog SendAppointmentConfirmation(string toEmail, string patientName, string doctorName, DateTime start, DateTime end, string reason)
    {
        string subject = "Medical appointment confirmation - San Vicente Hospital";
        string body = $@"
            Hello {patientName},

            Your appointment has been successfully scheduled ✅

            👨‍⚕️ Doctor: {doctorName}
            📅 Date: {start:dddd, dd MMM yyyy}
            🕒 Time: {start:HH:mm} - {end:HH:mm}
            📋 Reason: {reason}

            Thank you for trusting San Vicente Hospital 🏥";

        var log = new EmailLog
        {
            To = toEmail,
            Subject = subject,
            Body = body,
            DateSent = DateTime.Now
        };

        try
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_emailUser, _emailPass),
                EnableSsl = true
            };

            smtp.Send(_emailUser, toEmail, subject, body);

            log.Status = "Sent";
        }
        catch (Exception ex)
        {
            log.Status = "Not sent";
            log.ErrorMessage = ex.Message;
        }

        _emailRepo.Add(log);
        return log;
    }

    public void ViewEmailHistory()
    {
        var logs = _emailRepo.GetAll();

        if (!logs.Any())
        {
            Console.WriteLine("\n📭 No emails registered");
            return;
        }

        Console.WriteLine("\n--- 📧  EMAIL HISTORY ---");
        foreach (var log in logs)
        {
            Console.WriteLine($@"
            🆔 ID: {log.Id}
            📨 To: {log.To}
            📅 Date: {log.DateSent}
            📋 Subject: {log.Subject}
            📊 Status: {(log.Status == "Sent" ? "✅ Sent" : "❌ Not sent")}
            {(string.IsNullOrWhiteSpace(log.ErrorMessage) ? "" : $"⚠️ Error: {log.ErrorMessage}")}
            ──────────────────────────────────────────────");
        }
    }

}
