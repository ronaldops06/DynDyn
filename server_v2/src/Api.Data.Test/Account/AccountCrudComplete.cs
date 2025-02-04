using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Account
{
    public class AccountCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public AccountCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos(AccountEntity accountEntitySource, AccountEntity accountEntityDest)
        {
            Assert.NotNull(accountEntityDest);
            Assert.Equal(accountEntitySource.Name, accountEntityDest.Name);
            Assert.Equal(accountEntitySource.Status, accountEntityDest.Status);
            Assert.Equal(accountEntitySource.CategoryId, accountEntityDest.CategoryId);
            Assert.Equal(accountEntitySource.Category.Id, accountEntityDest.Category.Id);
            Assert.Equal(accountEntitySource.ParentAccountId, accountEntityDest.ParentAccountId);
            Assert.Equal(accountEntitySource.ParentAccount?.Id, accountEntityDest.ParentAccount?.Id);
            Assert.Equal(accountEntitySource.UserId, accountEntityDest.UserId);
            Assert.Equal(accountEntitySource.User.Id, accountEntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de Conta")]
        [Trait("CRUD", "AccountEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Conta()
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

                AccountRepository _repositorio = new AccountRepository(context);

                AccountEntity _parentAccountEntity = new AccountEntity()
                {
                    Name = "Geral",
                    Status = StatusType.Ativo,
                    CategoryId = _categoryCreated.Id,
                    Category = _categoryCreated,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _parentAccountCreated = await _repositorio.InsertAsync(_parentAccountEntity);
                AplicaTesteCampos(_parentAccountEntity, _parentAccountCreated);
                Assert.True(_parentAccountCreated.Id > 0);

                AccountEntity _accountEntity = new AccountEntity()
                {
                    Name = "Cash",
                    Status = StatusType.Ativo,
                    CategoryId = _categoryCreated.Id,
                    Category = _categoryCreated,
                    ParentAccountId = _parentAccountEntity.Id,
                    ParentAccount = _parentAccountEntity,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(_accountEntity);
                AplicaTesteCampos(_accountEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                _accountEntity.Status = StatusType.Inativo;

                var _registroAtualizado = await _repositorio.UpdateAsync(_accountEntity);
                AplicaTesteCampos(_accountEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(_accountEntity, _registroSelecionado);

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