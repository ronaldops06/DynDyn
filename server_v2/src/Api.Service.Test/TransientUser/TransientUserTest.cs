using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;
using Moq;
using Xunit;

namespace Api.Service.Test.TransientUser;

public class TransientUserTest : BaseTestService
{
    protected string AccessToken;
    protected Mock<ILoginService> LoginServiceMock = new Mock<ILoginService>();
    protected Mock<IUserService> UserServiceMock = new Mock<IUserService>();
    protected Mock<ICategoryService> CategoryServiceMock = new Mock<ICategoryService>();
    protected Mock<IOperationService> OperationServiceMock = new Mock<IOperationService>();
    protected Mock<ITransientUserRepository> RepositoryMock = new Mock<ITransientUserRepository>();
    protected TransientUserModel transientUserModel;
    protected TransientUserModel transientUserModelResult;
    protected CategoryModel categoryModel;
    protected OperationModel operationModel;

    protected TransientUserTest()
    {
        AccessToken = "kkjk3jjj3hhh3hh5h3kjhkjhdha.jh3hjhdhdjhdjhj";

        transientUserModel = new TransientUserModel()
        {
            Id = 1,
            Name = Faker.Name.FullName(),
            Login = Faker.Internet.Email(),
            VerificationCode = Faker.RandomNumber.Next(100000, 999999),
            ExpirationDate = DateTime.Now.AddMinutes(15),
        };

        transientUserModelResult = new TransientUserModel()
        {
            Id = transientUserModel.Id,
            Name = transientUserModel.Name,
            Login = transientUserModel.Login,
            DataCriacao = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow,
            VerificationCode = transientUserModel.VerificationCode,
            ExpirationDate = transientUserModel.ExpirationDate
        };

        categoryModel = new CategoryModel()
        {
            Name = "Transferência Contas",
            Status = StatusType.Ativo,
            Type = CategoryType.Operação,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        operationModel = new OperationModel()
        {
            Name = "Transferência Entre Contas",
            Status = StatusType.Ativo,
            Type = OperationType.Transferencia,
            CategoryId = categoryModel.Id,
            User = UserModelFake,
            UserId = UserModelFake.Id,
            Recurrent = false,
            Salary = false
        };
    }

    protected void ApplyTest(TransientUserModel userModelSource, TransientUserModel userModelDest)
    {
        Assert.NotNull(userModelDest);
        Assert.Equal(userModelSource.Id, userModelDest.Id);
        Assert.Equal(userModelSource.Name, userModelDest.Name);
        Assert.Equal(userModelSource.Login, userModelDest.Login);

        Assert.NotEqual(userModelSource.VerificationCode, userModelDest.VerificationCode);
        Assert.NotEqual(userModelSource.ExpirationDate, userModelDest.ExpirationDate);

        Assert.True(userModelDest.VerificationCode.ToString().Length == 6);
        // O teste não pode levar mais do que 2 minutos senão o teste abaixo irá falhar
        Assert.True(userModelDest.ExpirationDate >= DateTime.Now.AddMinutes(14));
        Assert.True(userModelDest.ExpirationDate <= DateTime.Now.AddMinutes(16));
    }
}