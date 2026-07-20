using System.Net;
using System.Text;
using System.Web;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Attribute
{
    public class WhenRequestAttribute : BaseTestAttribute
    {
        [Fact(DisplayName = "CRUD de Atributo")]
        public async Task Eh_Possivel_Realizar_Crud_Atributo()
        {
            await AdicionarToken();

            //Campos não informados no atributo
            var response = await PostJsonAsync(AttributeRequestDto, $"{HostApi}/Attribute", Client);
            var postResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Name é um campo obrigatório", postResult);
            
            //Campos não informados na opção de atributo ListOption
            
            //Atributo Não ListOption com opções
            GenerateRequestDto();
            
            response = await PostJsonAsync(AttributeRequestDto, $"{HostApi}/Attribute", Client);
            postResult = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Foi informado opção(oẽs) para um atributo que não é do tipo 'lista de opção'.", postResult);
            
            //Atributo ListOption sem opção
            AttributeRequestDto.DataType = (int)AttributeDataType.ListOptions;
            AttributeRequestDto.Options = null;
            
            response = await PostJsonAsync(AttributeRequestDto, $"{HostApi}/Attribute", Client);
            postResult = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Não foi informado a(s) opção(oẽs) para o atributo do tipo 'lista de opção'.", postResult);
            
            //Atributo ListOption com mais de uma opção default
            AttributeRequestDto.DataType = (int)AttributeDataType.ListOptions;
            
            List<AttributeOptionRequestDto> options = new List<AttributeOptionRequestDto>();
            var option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(2).Last(),
                IsDefault = 1,
                Status = (int)StatusType.Ativo
            };
            options.Add(option);
            
            option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(3).Last(),
                IsDefault = 1,
                Status = (int)StatusType.Ativo
            };
            
            options.Add(option);
            AttributeRequestDto.Options = options;
            
            response = await PostJsonAsync(AttributeRequestDto, $"{HostApi}/Attribute", Client);
            postResult = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("O atributo do tipo 'lista de opções' só pode ter uma opção default ativa.", postResult);

            options.Clear();
            
            //Atributo ListOption com mais de uma opção default, mas somente uma ativa
            option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(2).Last(),
                IsDefault = 1,
                Status = (int)StatusType.Inativo
            };
            options.Add(option);
            
            option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(3).Last(),
                IsDefault = 1,
                Status = (int)StatusType.Ativo
            };
            
            options.Add(option);
            AttributeRequestDto.Options = options;
            
            response = await PostJsonAsync(AttributeRequestDto, $"{HostApi}/Attribute", Client);
            postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<AttributeResponseDto>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(registroPost.Id == 0);
            Assert.Equal(AttributeRequestDto.Name, registroPost.Name);
            Assert.Equal(AttributeRequestDto.Status, registroPost.Status);
            Assert.Equal(AttributeRequestDto.DataType, registroPost.DataType);
            Assert.Equal(AttributeRequestDto.Description, registroPost.Description);
            Assert.True(registroPost.Options.Count() > 0);

            options.Clear();
            
            foreach (var optionPost in registroPost.Options)
            {
                option = new AttributeOptionRequestDto
                {
                    Id = optionPost.Id,
                    Label = optionPost.Label,
                    IsDefault = optionPost.IsDefault,
                    Status = optionPost.Status
                };
                options.Add(option);
            }

            AttributeRequestDto.Options = options;
            
            //GetAll
            var builder = new UriBuilder($"{HostApi}/Attribute");

            var query = HttpUtility.ParseQueryString(builder.Query);
            //Parâmetros específicos
            query[nameof(PageParams.PageNumber)] = $"{PageParams.PageNumber}";
            query[nameof(PageParams.PageSize)] = $"{PageParams.PageSize}";

            builder.Query = query.ToString();

            response = await Client.GetAsync(builder.Uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<AttributeResponseDto>>(jsonResult);

            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(r => r.Id == registroPost.Id).Count() == 1);

            //PUT
            AttributeRequestDto.Id = registroPost.Id;
            AttributeRequestDto.Description = Faker.Lorem.Sentence();

            var stringContent = new StringContent(JsonConvert.SerializeObject(AttributeRequestDto), Encoding.UTF8, "application/json");
            response = await Client.PutAsync($"{HostApi}/Attribute", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var registroUpdated = JsonConvert.DeserializeObject<AttributeResponseDto>(jsonResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.True(registroPost.Id == registroUpdated.Id);
            Assert.Equal(AttributeRequestDto.Name, registroUpdated.Name);
            Assert.Equal(AttributeRequestDto.Status, registroUpdated.Status);
            Assert.Equal(AttributeRequestDto.DataType, registroUpdated.DataType);
            Assert.Equal(AttributeRequestDto.Description, registroUpdated.Description);
            Assert.True(registroUpdated.Options.Count() > 0);

            //Delete
            response = await Client.DeleteAsync($"{HostApi}/Attribute/{registroUpdated.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}