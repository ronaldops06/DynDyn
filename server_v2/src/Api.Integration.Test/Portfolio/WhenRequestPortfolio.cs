using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Portfolio;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Portfolio
{
    public class WhenRequestPortfolio : BaseTestPortfolio
    {
        [Fact(DisplayName = "CRUD de Conta")]
        public async Task Eh_Possivel_Realizar_Crud_Conta()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync(PortfolioRequestDto, $"{HostApi}/Portfolio", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Name é um campo obrigatório", postResult);
            Assert.Contains("Status deve estar entre 0 e 1", postResult);

            GenerateRequestDto();

            //Post - Category
            response = await PostJsonAsync(CategoryRequestDto, $"{HostApi}/Category", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroCategoryPost = JsonConvert.DeserializeObject<CategoryResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroCategoryPost.Id == 0);

            CategoryRequestDto.Id = registroCategoryPost.Id;

            //Post - ParentPortfolio
            response = await PostJsonAsync(ParentPortfolioRequestDto, $"{HostApi}/Portfolio", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroParentPost = JsonConvert.DeserializeObject<PortfolioResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroParentPost.Id == 0);

            ParentPortfolioRequestDto.Id = registroParentPost.Id;

            //Post
            response = await PostJsonAsync(PortfolioRequestDto, $"{HostApi}/Portfolio", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<PortfolioResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(PortfolioBaseDto.Name, registroPost.Name);
            Assert.Equal(PortfolioBaseDto.Status, registroPost.Status);
            Assert.Equal(PortfolioBaseDto.Category.CategoryId, registroPost.Category.Id);
            Assert.Equal(PortfolioBaseDto.ParentPortfolio.Id, registroPost.ParentPortfolio.Id);
            Assert.Equal(DateTime.Now.Year, registroPost.DataCriacao?.Year);
            Assert.Equal(DateTime.Now.Month, registroPost.DataCriacao?.Month);
            Assert.Equal(DateTime.Now.Day, registroPost.DataCriacao?.Day);
            Assert.Equal(DateTime.Now.Hour, registroPost.DataCriacao?.Hour);

            //GetAll
            var builder = new UriBuilder($"{HostApi}/Portfolio");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query[nameof(PageParams.Tipo)] = $"{PageParams.Tipo}";
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<PortfolioResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            PortfolioRequestDto.Id = registroPost.Id;
            PortfolioRequestDto.Name = "Bradesco";

            var stringContent = new StringContent(JsonConvert.SerializeObject(PortfolioRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Portfolio", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            
            var registroUpdated = JsonConvert.DeserializeObject<PortfolioResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Name, registroUpdated.Name);
            Assert.Equal(PortfolioRequestDto.Name, registroUpdated.Name);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Portfolio/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}