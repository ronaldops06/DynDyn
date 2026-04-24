using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.{{model}};
using Api.Domain.Dtos.Category;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.{{model}}
{
    public class WhenRequest{{model}} : BaseTest{{model}}
    {
        [Fact(DisplayName = "CRUD de {{name}}")]
        public async Task Eh_Possivel_Realizar_Crud_Operacao()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync({{model}}RequestDto, $"{HostApi}/{{model}}", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Tratar campos obrigatórios

            GenerateRequestDto();

            //Post - Category
            //Referências

            //Post
            response = await PostJsonAsync({{model}}RequestDto, $"{HostApi}/{{model}}", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<{{model}}ResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            //

            //GetAll
            var builder = new UriBuilder($"{HostApi}/{{model}}");

            var query = HttpUtility.ParseQueryString(builder.Query);
            //Parâmetros específicos
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<{{model}}ResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            {{model}}RequestDto.Id = registroPost.Id;
            //Alterar campos

            var stringContent = new StringContent(JsonConvert.SerializeObject({{model}}RequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/{{model}}", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<{{model}}ResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            //Validar campos alterados

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/{{model}}/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}