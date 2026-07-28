using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.Brevo;
using Microsoft.Extensions.Options;

namespace Service.Services.Brevo
{
    public class BrevoEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly BrevoOptions _options;

        public BrevoEmailService(
            HttpClient httpClient,
            IOptions<BrevoOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task SendAsync( string email, string name, string subject, string html)
        {
            var apiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");
                
            var request = new BrevoEmailRequest
            {
                Sender = new EmailAddress
                {
                    Email = _options.SenderEmail,
                    Name = _options.SenderName
                },

                Subject = subject,

                HtmlContent = html
            };

            request.To.Add(new EmailAddress
            {
                Email = email,
                Name = name
            });

            var message = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.brevo.com/v3/smtp/email");

            message.Headers.Add("api-key", apiKey);
            
            var json = JsonSerializer.Serialize(request);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(message);

            response.EnsureSuccessStatusCode();
        }
    }
}