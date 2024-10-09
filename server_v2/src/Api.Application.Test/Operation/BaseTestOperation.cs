using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Application.Test.Helpers.BaseHelper;
using static Api.Application.Test.Helpers.OperationHelper;

namespace Api.Application.Test.Operation
{
    public class BaseTestOperation : BaseTestApplication
    {
        protected OperationController Controller;
        protected OperationModel OperationModel;
        protected OperationRequestDto OperationRequestDto;
        protected PageParams PageParams;
        protected List<OperationModel> ListOperationModel = new List<OperationModel>();

        protected BaseTestOperation()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Diversos",
                Type = CategoryType.Operação,
                Status = StatusType.Ativo
            };

            OperationModel = new OperationModel
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Recurrent = false,
                Type = GetOperationTypeRandom(),
                Status = GetStatusTypeRandom(),
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListOperationModel.Add(OperationModel);

            OperationModel = new OperationModel
            {
                Id = 2,
                Name = Faker.Name.FullName(),
                Recurrent = false,
                Type = GetOperationTypeRandom(),
                Status = GetStatusTypeRandom(),
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListOperationModel.Add(OperationModel);

            OperationModel = new OperationModel
            {
                Id = 3,
                Name = Faker.Name.FullName(),
                Recurrent = false,
                Type = GetOperationTypeRandom(),
                Status = GetStatusTypeRandom(),
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListOperationModel.Add(OperationModel);

            var categoryRequestDto = new CategoryRequestDto
            {
                Id = 1
            };

            OperationRequestDto = new OperationRequestDto
            {
                Name = OperationModel.Name,
                Recurrent = OperationModel.Recurrent,
                Type = (int)OperationModel.Type,
                Status = (int)OperationModel.Status,
                Category = categoryRequestDto,
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
    }
}