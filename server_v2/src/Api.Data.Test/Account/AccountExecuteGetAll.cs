using System.Globalization;
using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Api.Data.Test.Helpers.BaseHelper;

namespace Api.Data.Test.Account
{
    public class AccountExecuteGetAll : BaseTestGet<AccountEntity>, IClassFixture<DbTest>
    {
        public AccountExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Conta")]
        [Trait("GET", "AccountEntity")]
        public async Task Eh_Possivel_Realizar_Get_Conta()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
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
                Assert.NotNull(_parentAccountCreated);
                Assert.Equal(_parentAccountEntity.Name, _parentAccountCreated.Name);
                Assert.Equal(_parentAccountEntity.Status, _parentAccountCreated.Status);
                Assert.Equal(_parentAccountEntity.CategoryId, _parentAccountCreated.CategoryId);
                Assert.Equal(_parentAccountEntity.Category.Id, _parentAccountCreated.Category.Id);
                Assert.Equal(_parentAccountEntity.ParentAccountId, _parentAccountCreated.ParentAccountId);
                Assert.Equal(_parentAccountEntity.ParentAccount?.Id, _parentAccountCreated.ParentAccount?.Id);
                Assert.Equal(_parentAccountEntity.UserId, _parentAccountCreated.UserId);
                Assert.Equal(_parentAccountEntity.User.Id, _parentAccountCreated.User.Id);
                Assert.True(_parentAccountCreated.Id > 0);

                AccountRepository _accountRepository = new AccountRepository(context);

                for (int i = 1; i < RECORD_NUMBER; i++)
                {
                    AccountEntity _entity = new AccountEntity
                    {
                        Name = Faker.Name.FullName(),
                        Status = GetStatusTypeRandom(),
                        CategoryId = _categoryCreated.Id,
                        Category = _categoryCreated,
                        ParentAccountId = _parentAccountEntity.Id,
                        ParentAccount = _parentAccountEntity,
                        UserId = userCreated.Id,
                        User = userCreated
                    };

                    await _accountRepository.InsertAsync(_entity);
                }

                await RealizaGetPaginado(userCreated.Id, _accountRepository);

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                Thread.Sleep(1000);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    AccountEntity _entity = new AccountEntity
                    {
                        Name = Faker.Name.FullName(),
                        Status = GetStatusTypeRandom(),
                        CategoryId = _categoryCreated.Id,
                        Category = _categoryCreated,
                        ParentAccountId = _parentAccountEntity.Id,
                        ParentAccount = _parentAccountEntity,
                        UserId = userCreated.Id,
                        User = userCreated
                    };

                    await _accountRepository.InsertAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _accountRepository, lastSyncDate, 36);

                Thread.Sleep(1000);
                lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                Thread.Sleep(1000);

                //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
                for (int i = 10; i < (RECORD_NUMBER + 10); i++)
                {
                    AccountEntity _entity = await _accountRepository.SelectByIdAsync(userCreated.Id, i);

                    await _accountRepository.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _accountRepository, lastSyncDate, 10);
            }
        }
    }
}