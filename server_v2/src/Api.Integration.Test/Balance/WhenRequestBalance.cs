using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Balance;
using Api.Domain.Dtos.Category;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Balance
{
    public class WhenRequestBalance : BaseTestBalance
    {
        [Fact(DisplayName = "CRUD de Conta")]
        public async Task Eh_Possivel_Realizar_Crud_Conta()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync(BalanceRequestDto, $"{HostApi}/Balance", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Account é um campo obrigatório", postResult);

            GenerateRequestDto();

            //Post - Category
            response = await PostJsonAsync(CategoryRequestDto, $"{HostApi}/Category", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroCategoryPost = JsonConvert.DeserializeObject<CategoryResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroCategoryPost.Id == 0);

            CategoryRequestDto.Id = registroCategoryPost.Id;
            BalanceBaseDto.BalanceAccount.AccountCategory.CategoryId = registroCategoryPost.Id;

            //Post - Account
            response = await PostJsonAsync(AccountRequestDto, $"{HostApi}/Account", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroAccountPost = JsonConvert.DeserializeObject<AccountResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroAccountPost.Id == 0);

            AccountRequestDto.Id = registroAccountPost.Id;
            BalanceBaseDto.BalanceAccount.AccountId = registroAccountPost.Id;

            //Post
            response = await PostJsonAsync(BalanceRequestDto, $"{HostApi}/Balance", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<BalanceResponseDto>(postResult);
            
            BalanceBaseDto.BalanceId = registroPost.Id;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(BalanceBaseDto.BalanceId, registroPost.Id);
            Assert.Equal(BalanceBaseDto.BalanceValue, registroPost.Value);
            Assert.Equal(BalanceBaseDto.BalanceValuation, registroPost.Valuation);
            Assert.Equal(BalanceBaseDto.BalanceDividends, registroPost.Dividends);
            Assert.Equal(BalanceBaseDto.BalanceIncome, registroPost.Income);
            Assert.Equal(BalanceBaseDto.BalancePercentageValuation, registroPost.PercentageValuation);
            Assert.Equal(BalanceBaseDto.BalancePercentageIncome, registroPost.PercentageIncome);
            Assert.Equal(BalanceBaseDto.BalanceCredit, registroPost.Credit);
            Assert.Equal(BalanceBaseDto.BalanceDebit, registroPost.Debit);
            Assert.Equal(BalanceBaseDto.BalanceSalaryCredit, registroPost.SalaryCredit);
            Assert.Equal(BalanceBaseDto.BalanceSalaryDebit, registroPost.SalaryDebit);
            Assert.Equal(BalanceBaseDto.BalanceInflow, registroPost.Inflow);
            Assert.Equal(BalanceBaseDto.BalanceOutflow, registroPost.Outflow);
            Assert.NotNull(registroPost.Account);
            Assert.NotNull(registroPost.Account.Category);
            Assert.Equal(BalanceBaseDto.BalanceAccount.AccountId, registroPost.Account.Id);
            Assert.Equal(DateTime.Now.Year, registroPost.DataCriacao?.Year);
            Assert.Equal(DateTime.Now.Month, registroPost.DataCriacao?.Month);
            Assert.Equal(DateTime.Now.Day, registroPost.DataCriacao?.Day);
            Assert.Equal(DateTime.Now.Hour, registroPost.DataCriacao?.Hour);

            //GetAll
            var builder = new UriBuilder($"{HostApi}/Balance");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<BalanceResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            foreach (var record in listFromJson)
            {
                Assert.NotNull(record.Account);
                Assert.NotNull(record.Account.Category);
            }

            //PUT
            BalanceRequestDto.Id = registroPost.Id;
            BalanceRequestDto.Valuation = 155.54;
            BalanceRequestDto.Value = 8650.87;
            BalanceRequestDto.Credit = 5400;

            var stringContent = new StringContent(JsonConvert.SerializeObject(BalanceRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Balance", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<BalanceResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Valuation, registroUpdated.Valuation);
            Assert.Equal(BalanceRequestDto.Valuation, registroUpdated.Valuation);
            Assert.NotEqual(registroPost.Value, registroUpdated.Value);
            Assert.Equal(BalanceRequestDto.Value, registroUpdated.Value);
            Assert.NotEqual(registroPost.Credit, registroUpdated.Credit);
            Assert.Equal(BalanceRequestDto.Credit, registroUpdated.Credit);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Balance/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}