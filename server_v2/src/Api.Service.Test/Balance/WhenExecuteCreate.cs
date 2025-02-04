using Api.Domain.Entities;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.Balance
{
    public class WhenExecuteCreate : BalanceTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var balanceEntityResult = Mapper.Map<BalanceEntity>(balanceModelResult);
            var balanceEntity = Mapper.Map<BalanceEntity>(balanceModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(It.IsAny<BalanceEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<BalanceEntity>())).ReturnsAsync(balanceEntityResult);
            BalanceService service = new BalanceService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var result = await service.Post(balanceModel);
            ApplyTest(balanceModel, result);
        }
    }
}