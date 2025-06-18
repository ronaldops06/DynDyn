using Domain.Entities;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.TransientUser;

public class WhenExecuteVerificationCode : TransientUserTest
{
    [Fact(DisplayName = "É possível executar o método execute verification code.")]
    public async Task Eh_Possivel_Executar_Metodo_Verification_Code()
    {
        var userModel = Mapper.Map<UserModel>(transientUserModelResult);
        UserServiceMock.Setup(m => m.Post(It.IsAny<UserModel>())).ReturnsAsync(userModel);
        
        LoginServiceMock.Setup(m => m.GenerateToken(It.IsAny<TransientUserModel>())).Returns(AccessToken);
        
        var userEntityResult = Mapper.Map<TransientUserEntity>(transientUserModelResult);
        RepositoryMock.Setup(m => m.SelectUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userEntityResult);
        TransientUserService service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, Mapper);
        
        var result = await service.ExecuteVerificationCode(transientUserModelResult.Login, transientUserModelResult.VerificationCode ?? 0);
        
        Assert.NotNull(result);
        Assert.Equal(transientUserModelResult.Id, result.Id);
        Assert.Equal(transientUserModelResult.Name, result.Name);
        Assert.Equal(transientUserModelResult.Login, result.Login);
        
        Assert.Null(result.VerificationCode);
        Assert.Null(result.ExpirationDate);
        Assert.Equal(result.AccessToken, AccessToken);
    }

    [Fact(DisplayName = "Método execute verification code disparando exceções.")]
    public async Task Executando_Excecoes()
    {
        var userModel = Mapper.Map<UserModel>(transientUserModelResult);
        UserServiceMock.Setup(m => m.Post(It.IsAny<UserModel>())).ReturnsAsync(userModel);
        
        LoginServiceMock.Setup(m => m.GenerateToken(It.IsAny<TransientUserModel>())).Returns(AccessToken);
        
        TransientUserService service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, Mapper);
        
        var ex = await Assert.ThrowsAsync<Exception>(() => service.ExecuteVerificationCode(transientUserModelResult.Login, transientUserModelResult.VerificationCode ?? 0));
        Assert.Equal("Usuário não encontrado para validação, reinicie o cadastro.", ex.Message);
        
        var userEntityResult = Mapper.Map<TransientUserEntity>(transientUserModelResult);
        RepositoryMock.Setup(m => m.SelectUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userEntityResult);
        service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, Mapper);
        
        ex = await Assert.ThrowsAsync<Exception>(() => service.ExecuteVerificationCode(transientUserModelResult.Login, Faker.RandomNumber.Next(100000, 999999)));
        Assert.Equal("O código não corresponde ao código de verificação.", ex.Message);
        
        userEntityResult.ExpirationDate = DateTime.Now.AddMinutes(-1);
        RepositoryMock.Setup(m => m.SelectUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userEntityResult);
        service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, Mapper);
        
        ex = await Assert.ThrowsAsync<Exception>(() => service.ExecuteVerificationCode(transientUserModelResult.Login, transientUserModelResult.VerificationCode ?? 0));
        Assert.Equal("Código de validação expirado, reinicie o cadastro.", ex.Message);
    }
}