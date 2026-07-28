using Api.Domain.Dtos.Attribute;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.Attribute
{
    public class BaseTestAttribute : BaseIntegration
    {
        protected AttributeRequestDto AttributeRequestDto;
        protected PageParams PageParams;

        protected BaseTestAttribute()
        {
            PageParams = new PageParams()
            {
                Tipo = 2,
                PageNumber = 1,
                PageSize = 3
            };
            
            AttributeRequestDto = new AttributeRequestDto
            {
                Name = "",
                Description = Faker.Lorem.Sentence(),
                Status = (int)StatusType.Ativo,
                DataType = (int)AttributeDataType.Number,
            };
        }

        protected void GenerateRequestDto()
        {
            List<AttributeOptionRequestDto> options = new List<AttributeOptionRequestDto>();
            var option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(2).Last(),
                IsDefault = 0,
                Status = (int)StatusType.Ativo
            };
            
            options.Add(option);
            
            AttributeRequestDto = new AttributeRequestDto
            {
                Name = Faker.Lorem.Words(1).First(),
                Description = Faker.Lorem.Sentence(),
                Status = (int)StatusType.Ativo,
                DataType = (int)AttributeDataType.Number,
                Options = options
            };
        }
    }
}