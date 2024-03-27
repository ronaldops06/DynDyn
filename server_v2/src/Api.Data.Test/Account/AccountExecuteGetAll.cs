using Api.Data.Repository;
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
        private static readonly int RECORD_NUMBER = 35;

        public AccountExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Categoria")]
        [Trait("GET", "AccountEntity")]
        public async Task Eh_Possivel_Realizar_Get_Categoria()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                CategoryRepository _repositorioCategory = new CategoryRepository(context);
                CategoryEntity _categoryEntity = new CategoryEntity()
                {
                    Nome = "Corrente",
                    Tipo = CategoryType.Conta,
                    Status = StatusType.Ativo,
                };

                var _categoryCreated = await _repositorioCategory.InsertAsync(_categoryEntity);
                Assert.NotNull(_categoryCreated);
                Assert.True(_categoryCreated.Id > 0);
                Assert.Equal(_categoryEntity.Nome, _categoryCreated.Nome);
                Assert.Equal(_categoryEntity.Tipo, _categoryCreated.Tipo);
                Assert.Equal(_categoryEntity.Status, _categoryCreated.Status);

                AccountRepository _repositorio = new AccountRepository(context);

                AccountEntity _parentAccountEntity = new AccountEntity()
                {
                    Name = "Geral",
                    Status = StatusType.Ativo,
                    CategoryId = _categoryCreated.Id,
                    Category = _categoryCreated
                };

                var _parentAccountCreated = await _repositorio.InsertAsync(_parentAccountEntity);
                Assert.NotNull(_parentAccountCreated);
                Assert.Equal(_parentAccountEntity.Name, _parentAccountCreated.Name);
                Assert.Equal(_parentAccountEntity.Status, _parentAccountCreated.Status);
                Assert.Equal(_parentAccountEntity.CategoryId, _parentAccountCreated.CategoryId);
                Assert.Equal(_parentAccountEntity.Category.Id, _parentAccountCreated.Category.Id);
                Assert.Equal(_parentAccountEntity.ParentAccountId, _parentAccountCreated.ParentAccountId);
                Assert.Equal(_parentAccountEntity.ParentAccount?.Id, _parentAccountCreated.ParentAccount?.Id);
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
                        ParentAccount = _parentAccountEntity
                    };

                    await _accountRepository.InsertAsync(_entity);
                }

                await base.RealizaGetPaginado(_accountRepository);
            }
        }
    }
}