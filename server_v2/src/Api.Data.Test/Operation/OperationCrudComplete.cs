using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Operation
{
    public class OperationCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public OperationCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos(OperationEntity operationEntitySource, OperationEntity operationEntityDest)
        {
            Assert.NotNull(operationEntityDest);
            Assert.Equal(operationEntitySource.Name, operationEntityDest.Name);
            Assert.Equal(operationEntitySource.Recurrent, operationEntityDest.Recurrent);
            Assert.Equal(operationEntitySource.Type, operationEntityDest.Type);
            Assert.Equal(operationEntitySource.Status, operationEntityDest.Status);
            Assert.Equal(operationEntitySource.CategoryId, operationEntityDest.CategoryId);
            Assert.Equal(operationEntitySource.Category.Id, operationEntityDest.Category.Id);
            Assert.Equal(operationEntitySource.UserId, operationEntityDest.UserId);
            Assert.Equal(operationEntitySource.User.Id, operationEntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de Operação")]
        [Trait("CRUD", "OperationEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Operacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                CategoryRepository _repositorioCategory = new CategoryRepository(context);
                CategoryEntity _categoryEntity = new CategoryEntity()
                {
                    Name = "Corrente",
                    Type = CategoryType.Conta,
                    Status = StatusType.Ativo,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _categoryCreated = await _repositorioCategory.InsertAsync(_categoryEntity);
                Assert.NotNull(_categoryCreated);
                Assert.True(_categoryCreated.Id > 0);
                Assert.Equal(_categoryEntity.Name, _categoryCreated.Name);
                Assert.Equal(_categoryEntity.Type, _categoryCreated.Type);
                Assert.Equal(_categoryEntity.Status, _categoryCreated.Status);
                Assert.Equal(_categoryEntity.User.Id, _categoryCreated.User.Id);

                OperationRepository _repositorio = new OperationRepository(context);

                OperationEntity _operationEntity = new OperationEntity()
                {
                    Name = "Compra Monitor",
                    Recurrent = false,
                    Type = OperationType.Debito,
                    Status = StatusType.Ativo,
                    CategoryId = _categoryCreated.Id,
                    Category = _categoryCreated,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(_operationEntity);
                AplicaTesteCampos(_operationEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                _operationEntity.Status = StatusType.Inativo;
                _operationEntity.Type = OperationType.Credito;
                _operationEntity.Recurrent = true;

                var _registroAtualizado = await _repositorio.UpdateAsync(_operationEntity);
                AplicaTesteCampos(_operationEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(_operationEntity, _registroSelecionado);

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