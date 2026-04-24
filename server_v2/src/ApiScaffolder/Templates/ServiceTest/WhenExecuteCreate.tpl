using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.{{folder}}.{{model}}
{
    public class WhenExecuteCreate : {{model}}Test
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var {{alias}}EntityResult = Mapper.Map<{{model}}Entity>({{alias}}ModelResult);
            var {{alias}}Entity = Mapper.Map<{{model}}Entity>({{alias}}Model);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<{{model}}Type>())).ReturnsAsync(It.IsAny<{{model}}Entity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<{{model}}Entity>())).ReturnsAsync({{alias}}EntityResult);
            {{model}}Service service = new {{model}}Service(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Post({{alias}}Model);
            ApplyTest({{alias}}Model, result);
        }
    }
}