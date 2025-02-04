using Domain.Dtos.User;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteGet : UserTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            LoginServiceMock.Setup(m => m.GenerateToken(It.IsAny<UserModel>())).Returns(AccessToken);

            var userEntityResult = Mapper.Map<UserEntity>(userModelResult);
            var listUserEntity = Mapper.Map<List<UserEntity>>(listUserModelResult);

            var data = new Data<UserEntity>(listUserEntity.Count, listUserEntity);

            RepositoryMock.Setup(m => m.FindUsuarioByLogin(It.IsAny<string>())).ReturnsAsync(userEntityResult);
            RepositoryMock.Setup(m => m.FindUsuarioByUsernamaAndPassword(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(userEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync( It.IsAny<PageParams>())).ReturnsAsync(data);
            IUserService service = new UserService(RepositoryMock.Object, Mapper, LoginServiceMock.Object, null);

            var resultByLogin = await service.GetUsuarioByLogin(userModelResult.Login);
            ApplyTest(userModelResult, resultByLogin);

            var resultByUsernameAndPassword = await service.GetUsuarioByUsernameAndPassword(userModelResult);
            ApplyTest(userModelResult, resultByUsernameAndPassword);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);
        }
    }
}
