using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Api.Service.Test.Portfolio
{
    public class WhenExecuteUpdate : PortfolioTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var portfolioEntityUpdateResult = Mapper.Map<PortfolioEntity>(PortfolioModelUpdateResult);
            var portfolioEntityUpdate = Mapper.Map<PortfolioEntity>(PortfolioModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(),It.IsAny<string>(), It.IsAny<StatusType>())).ReturnsAsync(portfolioEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(portfolioEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<PortfolioEntity>())).ReturnsAsync(portfolioEntityUpdateResult);
            PortfolioService service = new PortfolioService(UserServiceMock.Object, RepositoryMock.Object, DeviceServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(PortfolioModelUpdate);
            ApplyTest(PortfolioModelUpdate, resultUpdate);
        }
    }
}