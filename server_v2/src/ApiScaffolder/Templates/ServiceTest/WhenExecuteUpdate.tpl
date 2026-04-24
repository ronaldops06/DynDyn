using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.{{folder}}.{{model}}
{
    public class WhenExecuteUpdate : {{model}}Test
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var {{alias}}EntityUpdateResult = Mapper.Map<{{model}}Entity>({{alias}}ModelUpdateResult);
            var {{alias}}EntityUpdate = Mapper.Map<{{model}}Entity>({{alias}}ModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<{{model}}Type>())).ReturnsAsync({{alias}}EntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync({{alias}}EntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<{{model}}Entity>())).ReturnsAsync({{alias}}EntityUpdateResult);
            {{model}}Service service = new {{model}}Service(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultUpdate = await service.Put({{alias}}ModelUpdate);
            ApplyTest({{alias}}ModelUpdate, resultUpdate);
        }
    }
}