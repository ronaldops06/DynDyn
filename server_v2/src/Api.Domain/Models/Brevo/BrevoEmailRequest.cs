using System.Collections.Generic;

namespace Domain.Models.Brevo
{
    public class BrevoEmailRequest
    {
        public EmailAddress Sender { get; set; } = null!;

        public List<EmailAddress> To { get; set; } = new List<EmailAddress>();

        public string Subject { get; set; } = "";

        public string HtmlContent { get; set; } = "";
    }
}