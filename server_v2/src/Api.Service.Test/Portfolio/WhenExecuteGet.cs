using Api.Service.Services;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Portfolio
{
    public class WhenExecuteGet : PortfolioTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var portfolioEntityResult = Mapper.Map<PortfolioEntity>(PortfolioModelResult);
            var listPortfolioEntity = Mapper.Map<List<PortfolioEntity>>(listPortfolioModelResult);

            var data = new Data<PortfolioEntity>(listPortfolioEntity.Count, listPortfolioEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(portfolioEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(),It.IsAny<PageParams>())).ReturnsAsync(data);
            PortfolioService service = new PortfolioService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultById = await service.GetById(PortfolioModelResult.Id);
            ApplyTest(PortfolioModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);
        }
    }
}