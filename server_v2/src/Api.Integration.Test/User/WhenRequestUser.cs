using Domain.Dtos.User;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace Api.Integration.Test.User
{
    public class WhenRequestUser : BaseIntegration
    {
        private class UserBase
        {
            public string UserName { get; set; }
            public string UserLogin { get; set; }
            public string UserRole { get; set; }
            public string UserPassword { get; set; }
            public int UserId { get; set; }
        }

        public static string GeneratePassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        [Fact(DisplayName = "CRUD de Usuário")]
        public async Task Eh_Possivel_Realizar_Crud_Usuario()
        {
            await AdicionarToken();
            var userBase = new UserBase()
            {
                UserName = Faker.Name.FullName(),
                UserLogin = Faker.Internet.Email(),
                UserPassword = GeneratePassword(8),
                UserRole = "Administrador",
            };

            var userDto = new UserDto
            {                
                Role = userBase.UserRole,
            };

            //Required
            var response = await PostJsonAsync(userDto, $"{HostApi}/User", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Name é um campo obrigatório", postResult);
            Assert.Contains("Login é um campo obrigatório", postResult);
            Assert.Contains("Password é um campo obrigatório", postResult);

            userDto.Name = userBase.UserName;
            userDto.Login = userBase.UserLogin;
            userDto.Password = userBase.UserPassword;

            //Post
            response = await PostJsonAsync(userDto, $"{HostApi}/User", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<UserResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(userBase.UserName, registroPost.Name);
            Assert.Equal(userBase.UserLogin, registroPost.Login);
            Assert.Equal(userBase.UserRole, registroPost.Role);

            /////PAREI AQUI, PRECISA INCLUIR A PAGINAÇÃO///////
            //GetAll
            response = await Client.GetAsync($"{HostApi}/User");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<UserResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            userDto.Id = registroPost.Id;
            userDto.Name = Faker.Name.FullName();

            var stringContent = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/User", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<UserResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Name, registroUpdated.Name);
            Assert.Equal(userDto.Name, registroUpdated.Name);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/User/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
