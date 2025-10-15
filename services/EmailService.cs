using System.Net;
using System.Net.Mail;
using SanVicenteHospital.models;
using SanVicenteHospital.repositories;
using SanVicenteHospital.interfaces;

namespace SanVicenteHospital.services;

public class EmailService
{
    private readonly IRepository<EmailLog> _emailRepo;

    public EmailService(IRepository<EmailLog> emailRepo)
    {
        _emailRepo = emailRepo;
    }

    public EmailLog SendAppointmentConfirmation(string toEmail, string patientName, string doctorName, DateTime start, DateTime end, string reason)
    {
        toEmail = "sebasreyes112@gmail.com";
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
                Credentials = new NetworkCredential("sebasreyes112@gmail.com", "olno czom qbzj dfwf"),
                EnableSsl = true
            };

            smtp.Send("sebasreyes112@gmail.com", toEmail, subject, body);

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
            Console.WriteLine("\n📭 No emails registered.");
            return;
        }

        Console.WriteLine("\n--- 📧 EMAIL HISTORY ---");
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
