using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Category;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Operation
{
    public class WhenRequestOperation : BaseTestOperation
    {
        [Fact(DisplayName = "CRUD de Operação")]
        public async Task Eh_Possivel_Realizar_Crud_Operacao()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync(OperationRequestDto, $"{HostApi}/Operation", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Name é um campo obrigatório", postResult);
            Assert.Contains("Status deve estar entre 0 e 1", postResult);
            Assert.Contains("Type deve estar entre 1 e 3", postResult);

            GenerateRequestDto();

            //Post - Category
            response = await PostJsonAsync(CategoryRequestDto, $"{HostApi}/Category", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroCategoryPost = JsonConvert.DeserializeObject<CategoryResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroCategoryPost.Id == 0);

            CategoryRequestDto.Id = registroCategoryPost.Id;

            //Post
            response = await PostJsonAsync(OperationRequestDto, $"{HostApi}/Operation", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<OperationResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(OperationBaseDto.OperationName, registroPost.Name);
            Assert.Equal(OperationBaseDto.OperationRecurrent, registroPost.Recurrent);
            Assert.Equal(OperationBaseDto.OperationType, registroPost.Type);
            Assert.Equal(OperationBaseDto.OperationStatus, registroPost.Status);
            Assert.Equal(OperationBaseDto.OperationCategory.CategoryId, registroPost.Category.Id);

            //GetAll
            var builder = new UriBuilder($"{HostApi}/Operation");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query[nameof(PageParams.Tipo)] = $"{PageParams.Tipo}";
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<OperationResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            OperationRequestDto.Id = registroPost.Id;
            OperationRequestDto.Name = "Compra de bicicleta";

            var stringContent = new StringContent(JsonConvert.SerializeObject(OperationRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Operation", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<OperationResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Name, registroUpdated.Name);
            Assert.Equal(OperationRequestDto.Name, registroUpdated.Name);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Operation/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}