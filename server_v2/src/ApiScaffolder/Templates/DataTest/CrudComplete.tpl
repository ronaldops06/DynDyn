using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.{{model}}
{
    public class {{model}}CrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public {{model}}CrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos({{model}}Entity {{model}}EntitySource, {{model}}Entity {{model}}EntityDest)
        {
            Assert.NotNull({{model}}EntityDest);
            //Adicionar os campos
            Assert.Equal({{model}}EntitySource.UserId, {{model}}EntityDest.UserId);
            Assert.Equal({{model}}EntitySource.User.Id, {{model}}EntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de {{name}}")]
        [Trait("CRUD", "{{model}}Entity")]
        public async Task Eh_Possivel_Realizar_CRUD_Operacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                //Referências

                {{model}}Repository _repositorio = new {{model}}Repository(context);

                {{model}}Entity _{{model}}Entity = new {{model}}Entity()
                {
                    
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(_{{model}}Entity);
                AplicaTesteCampos(_{{model}}Entity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                //Adicionar campos para update

                var _registroAtualizado = await _repositorio.UpdateAsync(_{{model}}Entity);
                AplicaTesteCampos(_{{model}}Entity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(_{{model}}Entity, _registroSelecionado);

                var _todosRegistros = await _repositorio.SelectAsync(userCreated.Id);
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