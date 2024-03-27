using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Account
{
    public class WhenExecuteCreate : AccountTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var accountEntityResult = Mapper.Map<AccountEntity>(accountModelResult);
            var accountEntity = Mapper.Map<AccountEntity>(accountModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<AccountEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<AccountEntity>())).ReturnsAsync(accountEntityResult);
            AccountService service = new AccountService(RepositoryMock.Object, Mapper);

            var result = await service.Post(accountModel);
            ApplyTest(accountModel, result);
        }
    }
}