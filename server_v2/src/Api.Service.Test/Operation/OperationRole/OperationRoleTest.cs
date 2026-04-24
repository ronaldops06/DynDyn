using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Domain.Models;
using Faker;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation.OperationRole
{
    public class OperationRoleTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<IOperationRoleRepository> RepositoryMock = new Mock<IOperationRoleRepository>();
        protected Mock<ITrashService> TrashServiceMock = new Mock<ITrashService>();
        protected List<OperationRoleModel> listOperationRoleModel = new List<OperationRoleModel>();
        protected List<OperationRoleModel> listOperationRoleModelResult = new List<OperationRoleModel>();
        protected OperationRoleModel operationRoleModel;
        protected OperationRoleModel operationRoleModelResult;
        protected OperationRoleModel operationRoleModelUpdate;
        protected OperationRoleModel operationRoleModelUpdateResult;
        protected PageParams pageParams;
        protected TrashModel trashModel;

        protected OperationRoleTest()
        {
            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new OperationRoleModel()
                {
                    Id = i,
                    Name = Name.FullName(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                listOperationRoleModel.Add(model);
            }

            listOperationRoleModelResult = listOperationRoleModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            operationRoleModel = new OperationRoleModel
            {
                Id = 2,
                Name = Name.FullName(),
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            operationRoleModelResult = new OperationRoleModel
            {
                Id = operationRoleModel.Id,
                Name = operationRoleModel.Name,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            operationRoleModelUpdate = new OperationRoleModel
            {
                Id = operationRoleModel.Id,
                Name = operationRoleModel.Name,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            operationRoleModelUpdateResult = new OperationRoleModel
            {
                Id = operationRoleModelUpdate.Id,
                Name = operationRoleModelUpdate.Name,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            trashModel = new TrashModel()
            {
                Reference = "operationRole",
                ReferenceId = operationRoleModel.Id
            };
        }

        protected void ApplyTest(OperationRoleModel operationRoleModelSource, OperationRoleModel operationRoleModelDest)
        {
            Assert.NotNull(operationRoleModelDest);
            Assert.NotNull(operationRoleModelDest.User);
            Assert.True(operationRoleModelDest.UserId > 0);
            Assert.Equal(operationRoleModelSource.Id, operationRoleModelDest.Id);
            Assert.Equal(operationRoleModelSource.Name, operationRoleModelDest.Name);
            Assert.Equal(operationRoleModelSource.UserId, operationRoleModelDest.UserId);
            Assert.Equal(operationRoleModelSource.User.Id, operationRoleModelDest.User.Id);
        }
    }
}