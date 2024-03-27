using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Account
{
    public class WhenExecuteDelete : AccountTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var accountEntity = Mapper.Map<AccountEntity>(accountModel);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>())).ReturnsAsync(accountEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            AccountService service = new AccountService(RepositoryMock.Object, Mapper);

            var result = await service.Delete(accountModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new AccountService(RepositoryMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}