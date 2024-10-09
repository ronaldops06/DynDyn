using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Category
{
    public class CategoryCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public CategoryCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "CRUD de Categoria")]
        [Trait("CRUD", "CategoryEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Categoria()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                CategoryRepository _repositorio = new CategoryRepository(context);
                CategoryEntity _categoryEntity = new CategoryEntity()
                {
                    Name = Faker.Lorem.GetFirstWord(),
                    Type = CategoryType.Conta,
                    Status = StatusType.Ativo,
                };

                var _registroCriado = await _repositorio.InsertAsync(_categoryEntity);
                Assert.NotNull(_registroCriado);
                Assert.True(_registroCriado.Id > 0);
                Assert.Equal(_categoryEntity.Name, _registroCriado.Name);
                Assert.Equal(_categoryEntity.Type, _registroCriado.Type);
                Assert.Equal(_categoryEntity.Status, _registroCriado.Status);

                _categoryEntity.Name = Faker.Lorem.GetFirstWord();
                _categoryEntity.Type = CategoryType.Operação;
                _categoryEntity.Status = StatusType.Inativo;

                var _registroAtualizado = await _repositorio.UpdateAsync(_categoryEntity);
                Assert.NotNull(_registroAtualizado);
                Assert.Equal(_registroCriado.Id, _registroAtualizado.Id);
                Assert.Equal(_categoryEntity.Name, _registroAtualizado.Name);
                Assert.Equal(_categoryEntity.Type, _registroAtualizado.Type);
                Assert.Equal(_categoryEntity.Status, _registroAtualizado.Status);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(_registroAtualizado.Id);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(_registroCriado.Id, _registroSelecionado.Id);
                Assert.Equal(_categoryEntity.Name, _registroSelecionado.Name);
                Assert.Equal(_categoryEntity.Type, _registroSelecionado.Type);
                Assert.Equal(_categoryEntity.Status, _registroSelecionado.Status);

                var _todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                var _removeu = await _repositorio.DeleteAsync(_registroCriado.Id);
                Assert.True(_removeu);

                _registroCriado.Id = 0;
                await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_registroCriado));
                await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_registroCriado.Id));
            }
        }
    }
}