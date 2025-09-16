using Api.Service.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Api.Service.Test.Portfolio
{
    public class WhenExecuteDelete : PortfolioTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var portfolioEntity = Mapper.Map<PortfolioEntity>(PortfolioModel);
            
            DeviceServiceMock.Setup(m => m.SendNotificationByUser(notificationModel));
            
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(portfolioEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            PortfolioService service = new PortfolioService(UserServiceMock.Object, RepositoryMock.Object, DeviceServiceMock.Object, Mapper);

            var result = await service.Delete(PortfolioModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new PortfolioService(UserServiceMock.Object, RepositoryMock.Object, DeviceServiceMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}