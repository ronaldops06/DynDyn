using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using Google.Apis.Auth.OAuth2;

namespace Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly string _projectId;
        private readonly GoogleCredential _googleCredential;
        
        public NotificationService()
        {
            _projectId = "sage-money";
            
            string jsonKey = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            _googleCredential = GoogleCredential.FromFile(jsonKey)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
        }
        
        
        public async Task SendMulticastMessageAsync(IEnumerable<string> targetTokens, string title, string body)
        {
            foreach (var token in targetTokens)
            {
                try
                {
                    await SendMessageAsync(token, title, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar para {token}: {ex.Message}");
                }
            }
        }


        public async Task SendMessageAsync(string targetToken, string title, string body)
        {
            var httpClient = new HttpClient();
            var accessToken = await GetAccessTokenAsync();
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var message = new
            {
                message = new
                {
                    token = targetToken,
                    notification = new
                    {
                        title = title,
                        body = body
                    }
                }
            };
            
            var json = JsonSerializer.Serialize(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send";
            var response = await httpClient.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao enviar notificação: {result}");
            }
        }
        
        private async Task<string> GetAccessTokenAsync() => await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
    }
}