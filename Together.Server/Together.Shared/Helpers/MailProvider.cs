using MimeKit;

namespace Together.Shared.Helpers;

public class MailForm
{
    public string To { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;
}

public sealed class MailConfig
{
    public string Host { get; set; } = default!;
    
    public int Port { get; set; }
    
    public string DisplayName { get; set; } = default!;
    
    public string Mail { get; set; } = default!;
    
    public string Password { get; set; } = default!;
}

public static class MailProvider
{
    public static async Task SmtpSendAsync(MailConfig config, MailForm form)
    {
        var mimeMessage = new MimeMessage();
        
        mimeMessage.Sender = new MailboxAddress(config.DisplayName, config.Mail);
        
        mimeMessage.From.Add(new MailboxAddress(config.DisplayName, config.Mail));
        
        mimeMessage.To.Add(MailboxAddress.Parse(form.To));
        
        mimeMessage.Subject = form.Title;
        
        mimeMessage.Body = new TextPart("html")
        {
            Text = form.Body
        };
        
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        
        try {
            await smtp.ConnectAsync(config.Host, config.Port, false);
            
            await smtp.AuthenticateAsync (config.Mail, config.Password);
            
            await smtp.SendAsync(mimeMessage);
            
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Có lỗi xảy ra", ex);
        }
    }
}