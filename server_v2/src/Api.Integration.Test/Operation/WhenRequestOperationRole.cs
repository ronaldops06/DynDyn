using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Operation;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Operation
{
    public class WhenRequestOperationRole : BaseTestOperationRole
    {
        [Fact(DisplayName = "CRUD de regras transação")]
        public async Task Eh_Possivel_Realizar_Crud_Operacao()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync(operationRoleRequestDto, $"{HostApi}/OperationRole", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Name é um campo obrigatório", postResult);

            GenerateRequestDto();
            
            //Post
            response = await PostJsonAsync(operationRoleRequestDto, $"{HostApi}/OperationRole", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<OperationRoleResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(operationRoleRequestDto.Name, registroPost.Name);

            //GetAll
            var builder = new UriBuilder($"{HostApi}/OperationRole");

            var query = HttpUtility.ParseQueryString(builder.Query);
            //Parâmetros específicos
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<OperationRoleResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            operationRoleRequestDto.Id = registroPost.Id;
            operationRoleRequestDto.Name = "Apartamento";

            var stringContent = new StringContent(JsonConvert.SerializeObject(operationRoleRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/OperationRole", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<OperationRoleResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Name, registroUpdated.Name);
            Assert.Equal(operationRoleRequestDto.Name, registroUpdated.Name);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/OperationRole/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}