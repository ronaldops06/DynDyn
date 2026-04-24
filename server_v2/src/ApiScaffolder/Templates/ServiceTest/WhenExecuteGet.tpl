using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.{{folder}}.{{model}}
{
    public class WhenExecuteGet : {{model}}Test
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var {{alias}}EntityResult = Mapper.Map<{{model}}Entity>({{alias}}ModelResult);
            var list{{model}}Entity = Mapper.Map<List<{{model}}Entity>>(list{{model}}ModelResult);

            var data = new Data<{{model}}Entity>(list{{model}}Entity.Count, list{{model}}Entity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync({{alias}}EntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<{{model}}Type>())).ReturnsAsync({{alias}}EntityResult);
            {{model}}Service service = new {{model}}Service(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultById = await service.GetById({{alias}}ModelResult.Id);
            ApplyTest({{alias}}ModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);

            //Métodos específicos
        }
    }
}