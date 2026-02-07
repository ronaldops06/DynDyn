using Api.Domain.Models;
using Domain.Entities;
using Domain.Models;
using Moq;
using Moq.Protected;
using Service.Services;
using Xunit;
using static Api.Service.Test.Helpers.BaseHelper;

namespace Api.Service.Test.TransientUser;

public class WhenExecuteCreate : TransientUserTest
{
    [Fact(DisplayName = "É possível executar o método Create.")]
    public async Task Eh_Possivel_Executar_Metodo_Create()
    {
        var userEntityResult = Mapper.Map<TransientUserEntity>(transientUserModelResult);
        
        UserModelFake = GetLoggedUserFake();
        UserServiceMock.Setup(m => m.GetLoggedUser()).ReturnsAsync(UserModelFake);
        
        RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransientUserEntity>())).ReturnsAsync(userEntityResult);
        
        //Implementado criando mock da própria classe pois o método SendMailVerification também precisa ser mocado.
        var service = new Mock<TransientUserService>(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, CategoryServiceMock.Object, OperationServiceMock.Object, Mapper);
        service.CallBase = true;
        service.Protected()
            .Setup<Task>("SendMailVerification", ItExpr.IsAny<string>(), ItExpr.IsAny<int?>(), ItExpr.IsAny<string>()).Returns(Task.CompletedTask);
        
        var result = await service.Object.Post(transientUserModel);
        ApplyTest(transientUserModel, result);
    }
    
    [Fact(DisplayName = "Método create disparando exceção usuário existente.")]
    public async Task Executando_Excecao_Usuario_Existe()
    {
        var userEntityResult = Mapper.Map<TransientUserEntity>(transientUserModelResult);
        
        var userModel = Mapper.Map<UserModel>(transientUserModel);
        UserServiceMock.Setup(m => m.GetUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userModel);

        CategoryServiceMock.Setup(m => m.GenerateInitialByUser(It.IsAny<UserModel>())).ReturnsAsync(categoryModel);
        OperationServiceMock.Setup(m => m.GenerateInitialByUser(It.IsAny<UserModel>(), It.IsAny<CategoryModel>()))
            .ReturnsAsync(operationModel);
        RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransientUserEntity>())).ReturnsAsync(userEntityResult);
        TransientUserService service = new TransientUserService(RepositoryMock.Object, UserServiceMock.Object, LoginServiceMock.Object, CategoryServiceMock.Object, OperationServiceMock.Object, Mapper);

        var ex = await Assert.ThrowsAsync<Exception>(() => service.Post(transientUserModel));
        Assert.Equal("Usuário não disponível.", ex.Message);
    }
}