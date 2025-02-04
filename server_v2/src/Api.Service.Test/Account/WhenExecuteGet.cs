using Api.Domain.Entities;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Account
{
    public class WhenExecuteGet : AccountTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var accountEntityResult = Mapper.Map<AccountEntity>(accountModelResult);
            var listAccountEntity = Mapper.Map<List<AccountEntity>>(listAccountModelResult);

            var data = new Data<AccountEntity>(listAccountEntity.Count, listAccountEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(accountEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(),It.IsAny<PageParams>())).ReturnsAsync(data);
            AccountService service = new AccountService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var resultById = await service.GetById(accountModelResult.Id);
            ApplyTest(accountModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);
        }
    }
}