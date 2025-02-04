using Domain.Entities;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteUpdate : UserTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var userEntityUpdateResult = Mapper.Map<UserEntity>(userModelUpdateResult);
            var userEntityUpdate = Mapper.Map<UserEntity>(userModelUpdate);

            LoginServiceMock.Setup(m => m.GenerateToken(It.IsAny<UserModel>())).Returns(AccessToken);

            RepositoryMock.Setup(m => m.FindUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>())).ReturnsAsync(userEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(userEntityUpdateResult);
            IUserService service = new UserService(RepositoryMock.Object, Mapper, LoginServiceMock.Object, null);

            var resultUpdate = await service.Put(userModelUpdate);
            ApplyTest(userModelUpdate, resultUpdate);
        }
    }
}
