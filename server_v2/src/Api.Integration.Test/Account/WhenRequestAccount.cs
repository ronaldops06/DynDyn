using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Category;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Account
{
    public class WhenRequestAccount : BaseTestAccount
    {
        [Fact(DisplayName = "CRUD de Conta")]
        public async Task Eh_Possivel_Realizar_Crud_Conta()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync(AccountRequestDto, $"{HostApi}/Account", Client);
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

            //Post - ParentAccount
            response = await PostJsonAsync(ParentAccountRequestDto, $"{HostApi}/Account", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroParentPost = JsonConvert.DeserializeObject<AccountResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroParentPost.Id == 0);

            ParentAccountRequestDto.Id = registroParentPost.Id;

            //Post
            response = await PostJsonAsync(AccountRequestDto, $"{HostApi}/Account", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<AccountResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(AccountBaseDto.AccountName, registroPost.Name);
            Assert.Equal(AccountBaseDto.AccountStatus, registroPost.Status);
            Assert.Equal(AccountBaseDto.AccountCategory.CategoryId, registroPost.Category.Id);
            Assert.Equal(AccountBaseDto.AccountParentAccount.AccountId, registroPost.ParentAccount.Id);

            //GetAll
            var builder = new UriBuilder($"{HostApi}/Account");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query[nameof(PageParams.Tipo)] = $"{PageParams.Tipo}";
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<AccountResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            AccountRequestDto.Id = registroPost.Id;
            AccountRequestDto.Name = "Bradesco";

            var stringContent = new StringContent(JsonConvert.SerializeObject(AccountRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Account", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<AccountResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Name, registroUpdated.Name);
            Assert.Equal(AccountRequestDto.Name, registroUpdated.Name);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Account/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}