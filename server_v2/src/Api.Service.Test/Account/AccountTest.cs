using Api.Domain.Enums;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Moq;
using Xunit;
using static Api.Service.Test.Helpers.BaseHelper;

namespace Api.Service.Test.Account
{
    public class AccountTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<IAccountRepository> RepositoryMock = new Mock<IAccountRepository>();
        protected List<AccountModel> listAccountModel = new List<AccountModel>();
        protected List<AccountModel> listAccountModelResult = new List<AccountModel>();
        protected AccountModel accountModel;
        protected AccountModel accountModelResult;
        protected AccountModel accountModelUpdate;
        protected AccountModel accountModelUpdateResult;
        protected PageParams pageParams;

        protected AccountTest()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Nome = "Corrente",
                Tipo = CategoryType.Conta,
                Status = StatusType.Ativo
            };

            var parentModel = new AccountModel
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel
            };

            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new AccountModel()
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Status = GetStatusTypeRandom(),
                    ParentAccountId = parentModel.Id,
                    ParentAccount = parentModel,
                    CategoryId = categoryModel.Id,
                    Category = categoryModel,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listAccountModel.Add(model);
            }

            listAccountModelResult = listAccountModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            accountModel = new AccountModel
            {
                Id = 2,
                Name = "Cash",
                Status = StatusType.Ativo,
                ParentAccountId = parentModel.Id,
                ParentAccount = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel
            };

            accountModelResult = new AccountModel
            {
                Id = accountModel.Id,
                Name = accountModel.Name,
                Status = accountModel.Status,
                ParentAccountId = accountModel.ParentAccountId,
                ParentAccount = accountModel.ParentAccount,
                CategoryId = accountModel.CategoryId,
                Category = accountModel.Category,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            accountModelUpdate = new AccountModel
            {
                Id = accountModel.Id,
                Name = "Carteira",
                Status = StatusType.Inativo,
                ParentAccountId = parentModel.Id,
                ParentAccount = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel
            };

            accountModelUpdateResult = new AccountModel
            {
                Id = accountModelUpdate.Id,
                Name = accountModelUpdate.Name,
                Status = accountModelUpdate.Status,
                ParentAccountId = accountModelUpdate.ParentAccountId,
                ParentAccount = accountModelUpdate.ParentAccount,
                CategoryId = accountModelUpdate.CategoryId,
                Category = accountModelUpdate.Category,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
        }

        protected void ApplyTest(AccountModel accountModelSource, AccountModel accountModelDest)
        {
            Assert.NotNull(accountModelDest);
            Assert.Equal(accountModelSource.Id, accountModelDest.Id);
            Assert.Equal(accountModelSource.Name, accountModelDest.Name);
            Assert.Equal(accountModelSource.Status, accountModelDest.Status);
            Assert.Equal(accountModelSource.CategoryId, accountModelDest.CategoryId);
            Assert.Equal(accountModelSource.Category.Id, accountModelDest.Category.Id);
            Assert.Equal(accountModelSource.ParentAccountId, accountModelDest.ParentAccountId);
            Assert.Equal(accountModelSource.ParentAccount.Id, accountModelDest.ParentAccount.Id);
        }
    }
}