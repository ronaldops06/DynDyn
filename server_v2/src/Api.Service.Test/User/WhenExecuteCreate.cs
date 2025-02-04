using Domain.Entities;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteCreate : UserTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var userEntityResult = Mapper.Map<UserEntity>(userModelResult);
            var userEntity = Mapper.Map<UserEntity>(userModel);

            LoginServiceMock.Setup(m => m.GenerateToken(It.IsAny<UserModel>())).Returns(AccessToken);

            RepositoryMock.Setup(m => m.FindUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(It.IsAny<UserEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<UserEntity>())).ReturnsAsync(userEntityResult);
            UserService service = new UserService(RepositoryMock.Object, Mapper, LoginServiceMock.Object, null);

            var result = await service.Post(userModel);
            ApplyTest(userModel, result);
        }
    }
}
