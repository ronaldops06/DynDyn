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
    protected Mock<ITransientUserRepository> RepositoryMock = new Mock<ITransientUserRepository>();
    protected TransientUserModel transientUserModel;
    protected TransientUserModel transientUserModelResult;

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
        Assert.True(userModelDest.ExpirationDate == new DateTime().AddMinutes(15));
    }
}