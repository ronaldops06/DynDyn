using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Domain.Models;
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
        protected Mock<IDeviceService> DeviceServiceMock = new Mock<IDeviceService>();
        protected List<OperationModel> listOperationModel = new List<OperationModel>();
        protected List<OperationModel> listOperationModelResult = new List<OperationModel>();
        protected OperationModel operationModel;
        protected OperationModel operationModelResult;
        protected OperationModel operationModelUpdate;
        protected OperationModel operationModelUpdateResult;
        protected PageParams pageParams;
        protected NotificationModel notificationModel;

        protected OperationTest()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Animais de Estimação",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo,
                User = UserModelFake,
                UserId = UserModelFake.Id
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
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
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
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
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
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            operationModelUpdate = new OperationModel
            {
                Id = operationModel.Id,
                Name = "Compra Comida",
                Recurrent = true,
                Type = operationModel.Type,
                Status = StatusType.Inativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
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
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            notificationModel = new NotificationModel
            {
                Title = "Exclude Entity",
                Body = new
                {
                    Operation = "DELETE",
                    Reference = "operation",
                    Id = operationModel.Id
                }
            };
        }

        protected void ApplyTest(OperationModel operationModelSource, OperationModel operationModelDest)
        {
            Assert.NotNull(operationModelDest);
            Assert.NotNull(operationModelDest.User);
            Assert.True(operationModelDest.UserId > 0);
            Assert.Equal(operationModelSource.Id, operationModelDest.Id);
            Assert.Equal(operationModelSource.Name, operationModelDest.Name);
            Assert.Equal(operationModelSource.Recurrent, operationModelDest.Recurrent);
            Assert.Equal(operationModelSource.Type, operationModelDest.Type);
            Assert.Equal(operationModelSource.Status, operationModelDest.Status);
            Assert.Equal(operationModelSource.CategoryId, operationModelDest.CategoryId);
            Assert.Equal(operationModelSource.Category.Id, operationModelDest.Category.Id);
            Assert.Equal(operationModelSource.UserId, operationModelDest.UserId);
            Assert.Equal(operationModelSource.User.Id, operationModelDest.User.Id);
        }
    }
}