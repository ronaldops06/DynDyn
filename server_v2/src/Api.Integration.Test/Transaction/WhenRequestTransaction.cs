using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Dtos.Transaction;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Transaction
{
    public class WhenRequestTransaction : BaseTestTransaction
    {
        [Fact(DisplayName = "CRUD de Transação")]
        public async Task Eh_Possivel_Realizar_Crud_Transacao()
        {
            await AdicionarToken();

            //Required
            var response = await PostJsonAsync(TransactionRequestDto, $"{HostApi}/Transaction", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Assert.Contains("Value é um campo obrigatório", postResult);
            //Assert.Contains("Consolidated é um campo obrigatório", postResult);
            Assert.Contains("Portfolio é um campo obrigatório", postResult);
            Assert.Contains("Operation é um campo obrigatório", postResult);

            GenerateRequestDto();

            //Post - CategoryAccount
            response = await PostJsonAsync(CategoryAccountRequestDto, $"{HostApi}/Category", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroCategoryPost = JsonConvert.DeserializeObject<CategoryResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroCategoryPost.Id == 0);

            CategoryAccountRequestDto.Id = registroCategoryPost.Id;

            //Post - CategoryOperation
            response = await PostJsonAsync(CategoryOperationRequestDto, $"{HostApi}/Category", Client);
            postResult = await response.Content.ReadAsStringAsync();
            registroCategoryPost = JsonConvert.DeserializeObject<CategoryResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroCategoryPost.Id == 0);

            CategoryOperationRequestDto.Id = registroCategoryPost.Id;

            //Post - ParentAccount
            response = await PostJsonAsync(ParentPortfolioAccountRequestDto, $"{HostApi}/Portfolio", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroAccountPost = JsonConvert.DeserializeObject<PortfolioResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroAccountPost.Id == 0);

            ParentPortfolioAccountRequestDto.Id = registroAccountPost.Id;
            
            //Post - Attribute lista de opções
            var attribute = GenerateAttributeListOption();
            
            response = await PostJsonAsync(attribute, $"{HostApi}/Attribute", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var attributeResponseDto = JsonConvert.DeserializeObject<AttributeResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(attributeResponseDto.Id == 0);
            
            //Atributo lista de opção com resposta (opção) para o atributo
            attribute.Id = attributeResponseDto.Id;
            var attributeOptionResponseDto = attributeResponseDto.Options.First();
            var attributeOptionRequestDto = new AttributeOptionRequestDto()
            {
              Id  = attributeOptionResponseDto.Id,
              Label = attributeOptionResponseDto.Label,
              IsDefault = attributeOptionResponseDto.IsDefault,
              Status = attributeOptionResponseDto.Status
            };
            AddAttributeInPortfolio(PortfolioAccountRequestDto, attribute, attributeOptionRequestDto);

            //Post - Account
            response = await PostJsonAsync(PortfolioAccountRequestDto, $"{HostApi}/Portfolio", Client);
            postResult = await response.Content.ReadAsStringAsync();
            registroAccountPost = JsonConvert.DeserializeObject<PortfolioResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroAccountPost.Id == 0);

            PortfolioAccountRequestDto.Id = registroAccountPost.Id;
            var portfolioAttributeResponseDto = registroAccountPost.Attributes.First();
            var portfolioAttributeRequestDto = PortfolioAccountRequestDto.Attributes.First();
            portfolioAttributeRequestDto.Id = portfolioAttributeResponseDto.Id;

            //Post - Operação não existente
            TransactionRequestDto.Operation.Id = 0;
            response = await PostJsonAsync(TransactionRequestDto, $"{HostApi}/Transaction", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<TransactionResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(TransactionRequestDto.Value, registroPost.Value);
            Assert.Equal(TransactionRequestDto.Observation, registroPost.Observation);
            Assert.Equal(TransactionRequestDto.Consolidated, registroPost.Consolidated);
            Assert.Equal(TransactionRequestDto.Installment, registroPost.Installment);
            Assert.Equal(TransactionRequestDto.TotalInstallments, registroPost.TotalInstallments);
            Assert.Equal(TransactionRequestDto.Portfolio.Id, registroPost.Portfolio.Id);
            Assert.Equal(1, registroPost.Operation.Id);  
            
            //Post - Operation
            OperationRequestDto.Name = "Curso de gaita";
            response = await PostJsonAsync(OperationRequestDto, $"{HostApi}/Operation", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroOperationPost = JsonConvert.DeserializeObject<OperationResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroOperationPost.Id == 0);

            OperationRequestDto.Id = registroOperationPost.Id;

            //Post com impacto
            TransactionRequestDto.Id = 0;
            AddTransactionImpact(PortfolioAccountRequestDto, portfolioAttributeRequestDto, 2);
            response = await PostJsonAsync(TransactionRequestDto, $"{HostApi}/Transaction", Client);
            postResult = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == HttpStatusCode.Created, postResult);
            registroPost = JsonConvert.DeserializeObject<TransactionResponseDto>(postResult);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(TransactionRequestDto.Value, registroPost.Value);
            Assert.Equal(TransactionRequestDto.Observation, registroPost.Observation);
            Assert.Equal(TransactionRequestDto.Consolidated, registroPost.Consolidated);
            Assert.Equal(TransactionRequestDto.Installment, registroPost.Installment);
            Assert.Equal(TransactionRequestDto.TotalInstallments, registroPost.TotalInstallments);
            Assert.Equal(TransactionRequestDto.Portfolio.Id, registroPost.Portfolio.Id);
            Assert.Equal(TransactionRequestDto.Operation.Id, registroPost.Operation.Id);
            Assert.Equal(TransactionRequestDto.Impacts.Count(), registroPost.Impacts.Count());
            Assert.Equal(TransactionRequestDto.Impacts.First().Portfolio.Id, registroPost.Impacts.First().Portfolio.Id);
            Assert.Equal(TransactionRequestDto.Impacts.First().PortfolioAttribute.Id, registroPost.Impacts.First().PortfolioAttribute.Id);
            Assert.Equal(TransactionRequestDto.Impacts.First().ImpactType, registroPost.Impacts.First().ImpactType);
            Assert.Equal(TransactionRequestDto.Impacts.First().OperationType, registroPost.Impacts.First().OperationType);
            Assert.Equal(TransactionRequestDto.Impacts.First().Value, registroPost.Impacts.First().Value);
            
            //GetAll
            var builder = new UriBuilder($"{HostApi}/Transaction");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query[nameof(PageParams.DataCriacaoInicio)] = $"{PageParams.DataCriacaoInicio.Value.ToString("yyyy-MM-dd HH:mm:ss")}";
            query[nameof(PageParams.DataCriacaoFim)] = $"{PageParams.DataCriacaoFim.Value.ToString("yyyy-MM-dd HH:mm:ss")}";
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<TransactionResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            TransactionRequestDto.Id = registroPost.Id;
            TransactionRequestDto.Value = 1500.00;
            TransactionRequestDto.Observation = "Testes de atualização";

            var stringContent = new StringContent(JsonConvert.SerializeObject(TransactionRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Transaction", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<TransactionResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(registroPost.Value, registroUpdated.Value);
            Assert.NotEqual(registroPost.Observation, registroUpdated.Observation);
            Assert.Equal(TransactionRequestDto.Value, registroUpdated.Value);
            Assert.Equal(TransactionRequestDto.Observation, registroUpdated.Observation);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Transaction/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
