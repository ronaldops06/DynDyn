using Api.Domain.Enums;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Moq;
using Xunit;
using static Api.Service.Test.Helpers.BaseHelper;
using static Api.Service.Test.Helpers.OperationHelper;

namespace Api.Service.Test.Operation
{
    public class OperationTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<IOperationRepository> RepositoryMock = new Mock<IOperationRepository>();
        protected List<OperationModel> listOperationModel = new List<OperationModel>();
        protected List<OperationModel> listOperationModelResult = new List<OperationModel>();
        protected OperationModel operationModel;
        protected OperationModel operationModelResult;
        protected OperationModel operationModelUpdate;
        protected OperationModel operationModelUpdateResult;
        protected PageParams pageParams;

        protected OperationTest()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Animais de Estimação",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo
            };

            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new OperationModel()
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Recurrent = false,
                    Type = GetOperationTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    CategoryId = categoryModel.Id,
                    Category = categoryModel,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listOperationModel.Add(model);
            }

            listOperationModelResult = listOperationModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            operationModel = new OperationModel
            {
                Id = 2,
                Name = "Compra Bicicleta",
                Recurrent = false,
                Type = OperationType.Debito,
                Status = StatusType.Ativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel
            };

            operationModelResult = new OperationModel
            {
                Id = operationModel.Id,
                Name = operationModel.Name,
                Recurrent = operationModel.Recurrent,
                Type = operationModel.Type,
                Status = operationModel.Status,
                CategoryId = operationModel.CategoryId,
                Category = operationModel.Category,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            operationModelUpdate = new OperationModel
            {
                Id = operationModel.Id,
                Name = "Compra Comida",
                Recurrent = true,
                Type = operationModel.Type,
                Status = StatusType.Inativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel
            };

            operationModelUpdateResult = new OperationModel
            {
                Id = operationModelUpdate.Id,
                Name = operationModelUpdate.Name,
                Recurrent = operationModelUpdate.Recurrent,
                Type = operationModelUpdate.Type,
                Status = operationModelUpdate.Status,
                CategoryId = operationModelUpdate.CategoryId,
                Category = operationModelUpdate.Category,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
        }

        protected void ApplyTest(OperationModel operationModelSource, OperationModel operationModelDest)
        {
            Assert.NotNull(operationModelDest);
            Assert.Equal(operationModelSource.Id, operationModelDest.Id);
            Assert.Equal(operationModelSource.Name, operationModelDest.Name);
            Assert.Equal(operationModelSource.Recurrent, operationModelDest.Recurrent);
            Assert.Equal(operationModelSource.Type, operationModelDest.Type);
            Assert.Equal(operationModelSource.Status, operationModelDest.Status);
            Assert.Equal(operationModelSource.CategoryId, operationModelDest.CategoryId);
            Assert.Equal(operationModelSource.Category.Id, operationModelDest.Category.Id);
        }
    }
}