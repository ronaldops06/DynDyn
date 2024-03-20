using Api.Domain.Dtos.Category;
using Domain.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using Xunit;

namespace Api.Integration.Test
{
    public class WhenRequestCategory : BaseIntegration
    {
        private class CategoryBase
        {
            public string CategoryNome { get; set; }
            public int CategoryTipo { get; set; }
            public int CategoryStatus { get; set; }
            public int CategoryId { get; set; }
        }

        [Fact(DisplayName = "CRUD de Categoria")]
        public async Task Eh_Possivel_Realizar_Crud_Usuario()
        {
            await AdicionarToken();

            var pageParams = new PageParams()
            {
                Tipo = 1,
                PageNumber = 1,
                PageSize = 3
            };

            var categoryBase = new CategoryBase()
            {
                CategoryNome = Faker.Name.FullName(),
                CategoryTipo = 1,
                CategoryStatus = 1
            };

            var categoryRequestDto = new CategoryRequestDto
            {
                Nome = null,
                Tipo = 0,
                Status = 3
            };

            //Required
            var response = await PostJsonAsync(categoryRequestDto, $"{HostApi}/Category", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Nome é um campo obrigatório", postResult);
            Assert.Contains("Tipo deve estar entre 1 e 2", postResult);
            Assert.Contains("Status deve estar entre 0 e 1", postResult);

            categoryRequestDto.Nome = categoryBase.CategoryNome;
            categoryRequestDto.Tipo = (int)categoryBase.CategoryTipo;
            categoryRequestDto.Status = (int)categoryBase.CategoryStatus;

            //Post
            response = await PostJsonAsync(categoryRequestDto, $"{HostApi}/Category", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<CategoryResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(categoryBase.CategoryNome, registroPost.Nome);
            Assert.Equal(categoryBase.CategoryTipo, registroPost.Tipo);
            Assert.Equal(categoryBase.CategoryStatus, registroPost.Status);

            //GetAll
            var builder = new UriBuilder($"{HostApi}/Category");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query[nameof(PageParams.Tipo)] = $"{pageParams.Tipo}";
            query[nameof(PageParams.PageNumber)] = $"{pageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{pageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<CategoryResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            categoryRequestDto.Id = registroPost.Id;
            categoryRequestDto.Nome = Faker.Name.FullName();

            var stringContent = new StringContent(JsonConvert.SerializeObject(categoryRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Category", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<CategoryResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Nome, registroUpdated.Nome);
            Assert.Equal(categoryRequestDto.Nome, registroUpdated.Nome);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Category/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
