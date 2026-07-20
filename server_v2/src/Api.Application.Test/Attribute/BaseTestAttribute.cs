using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Application.Test.Helpers.BaseHelper;
using static Api.Application.Test.Helpers.AttributeHelper;

namespace Api.Application.Test
{
    public class BaseTestAttribute : BaseTestApplication
    {
        protected AttributeController Controller;
        protected AttributeModel AttributeModel;
        protected AttributeRequestDto AttributeRequestDto;
        protected PageParams PageParams;
        protected List<AttributeModel> ListAttributeModel = new List<AttributeModel>();

        protected BaseTestAttribute()
        {
            //Referências

            AttributeModel = new AttributeModel
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Description = Faker.Lorem.Sentence(),
                Status = GetStatusTypeRandom(),
                DataType = GetAttributeDataTypeRandom(),
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListAttributeModel.Add(AttributeModel);

            AttributeModel = new AttributeModel
            {
                Id = 2,
                Name = Faker.Name.FullName(),
                Description = Faker.Lorem.Sentence(),
                Status = GetStatusTypeRandom(),
                DataType = GetAttributeDataTypeRandom(),
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListAttributeModel.Add(AttributeModel);

            AttributeModel = new AttributeModel
            {
                Id = 3,
                Name = Faker.Name.FullName(),
                Description = Faker.Lorem.Sentence(),
                Status = GetStatusTypeRandom(),
                DataType = GetAttributeDataTypeRandom(),
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListAttributeModel.Add(AttributeModel);
            
            AttributeRequestDto = new AttributeRequestDto
            {
                Name = AttributeModel.Name,
                Description = AttributeModel.Description,
                Status = (int)AttributeModel.Status,
                DataType = (int)AttributeModel.DataType,
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
    }
}