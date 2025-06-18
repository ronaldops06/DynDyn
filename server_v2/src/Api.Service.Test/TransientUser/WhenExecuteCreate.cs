using Domain.Entities;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.TransientUser;

public class WhenExecuteCreate : TransientUserTest
{
    [Fact(DisplayName = "É possível executar o método Create.")]
    public async Task Eh_Possivel_Executar_Metodo_Create()
    {
        var userEntityResult = Mapper.Map<TransientUserEntity>(transientUserModelResult);
        
        RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransientUserEntity>())).ReturnsAsync(userEntityResult);
        TransientUserService service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, Mapper);
        
        var result = await service.Post(transientUserModel);
        ApplyTest(transientUserModel, result);
    }
    
    [Fact(DisplayName = "Método create disparando exceção usuário existente.")]
    public async Task Executando_Excecao_Usuario_Existe()
    {
        var userEntityResult = Mapper.Map<TransientUserEntity>(transientUserModelResult);
        
        var userModel = Mapper.Map<UserModel>(transientUserModel);
        UserServiceMock.Setup(m => m.GetUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userModel);
        
        RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransientUserEntity>())).ReturnsAsync(userEntityResult);
        TransientUserService service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, Mapper);

        var ex = await Assert.ThrowsAsync<Exception>(() => service.Post(transientUserModel));
        Assert.Equal("Usuário não disponível.", ex.Message);
    }
}