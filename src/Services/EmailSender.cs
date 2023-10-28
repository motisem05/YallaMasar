using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using YallaMasar.Configurations;

namespace YallaMasar.Services;
public interface IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
{
	Task SendEmailAsync(string name, string email, string subject, string htmlMessage, string from = null);
}

public class EmailSender : IEmailSender
{
	private readonly SmtpConfigurations _smtpConfigurations;

	public EmailSender(SmtpConfigurations smtpConfigurations)
	{
		_smtpConfigurations = smtpConfigurations;
	}

	public async Task SendEmailAsync(string name, string email, string subject, string htmlMessage, string from = null)
	{
		var message = new MimeMessage();

		string fromName = "";

		if (!string.IsNullOrEmpty(from))
			fromName = $"{from} - ";

		fromName += $" {_smtpConfigurations.FromName}";

		message.From.Add(new MailboxAddress(fromName, _smtpConfigurations.FromEmail));

		message.To.Add(new MailboxAddress(name, email));

		message.Subject = subject;

		message.Body = new BodyBuilder
		{
			HtmlBody = htmlMessage
		}.ToMessageBody();

		using var client = new SmtpClient();

		client.Connect(_smtpConfigurations.Host, _smtpConfigurations.Port, SecureSocketOptions.Auto);

		// Note: only needed if the SMTP server requires authentication
		await client.AuthenticateAsync(_smtpConfigurations.Username, _smtpConfigurations.Password);

		await client.SendAsync(message);

		await client.DisconnectAsync(true);
	}

	public async Task SendEmailAsync(string email, string subject, string htmlMessage)
	{
		var message = new MimeMessage();

		message.From.Add(new MailboxAddress(_smtpConfigurations.FromName, _smtpConfigurations.FromEmail));

		message.To.Add(new MailboxAddress(email, email));

		message.Subject = subject;

		message.Body = new BodyBuilder
		{
			HtmlBody = htmlMessage
		}.ToMessageBody();

		using var client = new SmtpClient();

		client.Connect(_smtpConfigurations.Host, _smtpConfigurations.Port, SecureSocketOptions.Auto);

		// Note: only needed if the SMTP server requires authentication
		await client.AuthenticateAsync(_smtpConfigurations.Username, _smtpConfigurations.Password);

		await client.SendAsync(message);

		await client.DisconnectAsync(true);
	}
}
