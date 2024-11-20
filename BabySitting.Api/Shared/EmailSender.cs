using System.Net.Mail;
using System.Net;
using BabySitting.Api.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Encodings.Web;

namespace BabySitting.Api.Shared;

public interface ISenderEmail
{
    Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false);
    Task SendConfirmationEmail(string email, User user);
}
public class EmailSender : ISenderEmail
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public EmailSender(IConfiguration configuration, UserManager<User> userManager) 
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
    {
        string MailServer = _configuration["EmailSettings:MailServer"]!;
        string FromEmail = _configuration["EmailSettings:FromEmail"]!;
        string Password = _configuration["EmailSettings:Password"]!;
        int Port = int.Parse(_configuration["EmailSettings:MailPort"]!);
        var client = new SmtpClient(MailServer, Port)
        {
            Credentials = new NetworkCredential(FromEmail, Password),
            EnableSsl = true,
        };
        MailMessage mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Body)
        {
            IsBodyHtml = IsBodyHtml
        };
        return client.SendMailAsync(mailMessage);
    }

    public async Task SendConfirmationEmail(string email, User user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string baseUrl = _configuration["AppBaseUrl"]!;
        string confirmationLink = $"{baseUrl}/Account/ConfirmEmail?UserId={user.Id}&Token={token}";
        string encodedLink = HtmlEncoder.Default.Encode(confirmationLink);
        await SendEmailAsync(email, "Confirm Your Email", $"Please confirm your account by <a href='{encodedLink}'>clicking here</a>.", true);
    }

}
