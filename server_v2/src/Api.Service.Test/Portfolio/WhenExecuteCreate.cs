using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Api.Service.Test.Portfolio
{
    public class WhenExecuteCreate : PortfolioTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var portfolioEntityResult = Mapper.Map<PortfolioEntity>(PortfolioModelResult);
            var portfolioEntity = Mapper.Map<PortfolioEntity>(PortfolioModel);
            
            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<StatusType>())).ReturnsAsync(It.IsAny<PortfolioEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<PortfolioEntity>())).ReturnsAsync(portfolioEntityResult);
            PortfolioService service = new PortfolioService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var result = await service.Post(PortfolioModel);
            ApplyTest(PortfolioModel, result);
        }
    }
}