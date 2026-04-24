using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Operation
{
    public class OperationRoleCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public OperationRoleCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos(OperationRoleEntity operationRoleEntitySource, OperationRoleEntity operationRoleEntityDest)
        {
            Assert.NotNull(operationRoleEntityDest);
            Assert.Equal(operationRoleEntitySource.Name, operationRoleEntityDest.Name);
            Assert.Equal(operationRoleEntitySource.UserId, operationRoleEntityDest.UserId);
            Assert.Equal(operationRoleEntitySource.User.Id, operationRoleEntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de papel de operação")]
        [Trait("CRUD", "OperationRoleEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Papel_Operacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                OperationRoleRepository _repositorio = new OperationRoleRepository(context);

                OperationRoleEntity operationRoleEntity = new OperationRoleEntity()
                {
                    Name = "FGTS",
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(operationRoleEntity);
                AplicaTesteCampos(operationRoleEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                operationRoleEntity.Name = "Carro";

                var _registroAtualizado = await _repositorio.UpdateAsync(operationRoleEntity);
                AplicaTesteCampos(operationRoleEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(operationRoleEntity, _registroSelecionado);

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