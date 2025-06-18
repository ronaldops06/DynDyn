using AutoMapper;
using CrossCutting.Mappings;
using Data.Context;
using Domain.Dtos.User;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Api.Integration.Test
{
    public class BaseIntegration : IDisposable
    {
        private static readonly string BEARER_TOKEN = "Bearer";

        public SomniaContext SomniaContext { get; private set; }
        public HttpClient Client { get; private set; }
        public IMapper Mapper { get; set; }
        public string HostApi { get; set; }
        public HttpResponseMessage Response { get; set; }

        public BaseIntegration()
        {
            HostApi = "http://127.0.0.1:5000/api/v1";

            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<FakeStartup>();
            var server = new TestServer(builder);

            SomniaContext = (SomniaContext)server.Host.Services.GetService(typeof(SomniaContext));

            Mapper = new AutoMapperFixture().GetMapper();

            Client = server.CreateClient();
        }

        public async Task AdicionarToken()
        {
            var loginDto = new LoginDto
            {
                Login = "admin@gmail.com",
                Password = "pgadmin"
            };

            var resultLogin = await PostJsonAsync(loginDto, $"{HostApi}/Login/Auth", Client);
            var jsonLogin = await resultLogin.Content.ReadAsStringAsync();
            Assert.NotNull(jsonLogin);
            var loginObject = JsonConvert.DeserializeObject<LoginResponseDto>(jsonLogin);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER_TOKEN, loginObject.AccessToken);
        }
        
        public async Task AdicionarTokenUsuarioAdicional()
        {
            var loginDto = new LoginDto
            {
                Login = "ope@gmail.com",
                Password = "pgadmin"
            };

            var resultLogin = await PostJsonAsync(loginDto, $"{HostApi}/Login/Auth", Client);
            var jsonLogin = await resultLogin.Content.ReadAsStringAsync();
            var loginObject = JsonConvert.DeserializeObject<LoginResponseDto>(jsonLogin);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER_TOKEN, loginObject.AccessToken);
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(object dataclass, string url, HttpClient client)
        {
            return await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(dataclass), Encoding.UTF8, "application/json"));
        }

        public void Dispose()
        {
            SomniaContext.Dispose();
            Client.Dispose();
        }
    }

    public class AutoMapperFixture : IDisposable
    {
        public IMapper GetMapper()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DtoToModelProfile());
                cfg.AddProfile(new EntityToModelProfile());
            });

            return config.CreateMapper();
        }
        public void Dispose() { }
    }
}
