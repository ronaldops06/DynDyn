using Domain.Entities;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteDelete : UserTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            LoginServiceMock.Setup(m => m.GenerateToken(It.IsAny<UserModel>())).Returns(AccessToken);

            var userEntity = Mapper.Map<UserEntity>(userModel);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>())).ReturnsAsync(userEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            IUserService service = new UserService(RepositoryMock.Object, Mapper, LoginServiceMock.Object, null);

            var result = await service.Delete(userModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new UserService(RepositoryMock.Object, Mapper, LoginServiceMock.Object, null);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}
