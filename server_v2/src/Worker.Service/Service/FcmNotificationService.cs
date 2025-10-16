using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Worker.Service.Interfaces;

namespace Worker.Service.Service;

public class FcmNotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly string _projectId;
    private readonly GoogleCredential _googleCredential;
    
    public FcmNotificationService(INotificationRepository repository)
    {
        _repository = repository;
        _projectId = "sage-money";
            
        string jsonKey = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        Console.WriteLine(jsonKey);
        _googleCredential = GoogleCredential.FromFile(jsonKey)
            .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
    }
    
    public async Task SendPendingNotificationsAsync()
    {
        var notifications = await _repository.SelectPendingNotificationsAsync();
        
        foreach (var notification in notifications)
        {
            await SendMessageAsync(notification.DeviceToken, notification.Title, notification.Message);

            notification.Sent = true;
            await _repository.UpdateAsync(notification);
        }
    }
    
    private async Task SendMessageAsync(string targetToken, string title, string body)
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