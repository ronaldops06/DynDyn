using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Account
{
    public class WhenExecuteUpdate : AccountTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var accountEntityUpdateResult = Mapper.Map<AccountEntity>(accountModelUpdateResult);
            var accountEntityUpdate = Mapper.Map<AccountEntity>(accountModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<string>())).ReturnsAsync(accountEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>())).ReturnsAsync(accountEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<AccountEntity>())).ReturnsAsync(accountEntityUpdateResult);
            AccountService service = new AccountService(RepositoryMock.Object, Mapper);

            var resultUpdate = await service.Put(accountModelUpdate);
            ApplyTest(accountModelUpdate, resultUpdate);
        }
    }
}