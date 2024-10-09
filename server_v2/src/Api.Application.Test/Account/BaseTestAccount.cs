using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.Account
{
    public class BaseTestAccount : BaseTestApplication
    {
        protected AccountController Controller;
        protected AccountModel AccountModel;
        protected AccountRequestDto AccountRequestDto;
        protected PageParams PageParams;
        protected List<AccountModel> ListAccountModel = new List<AccountModel>();

        protected BaseTestAccount()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Corrente",
                Type = CategoryType.Conta,
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

            AccountModel = new AccountModel
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Status = GetStatusTypeRandom(),
                ParentAccountId = parentModel.Id,
                ParentAccount = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListAccountModel.Add(AccountModel);

            AccountModel = new AccountModel
            {
                Id = 2,
                Name = Faker.Name.FullName(),
                Status = GetStatusTypeRandom(),
                ParentAccountId = parentModel.Id,
                ParentAccount = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListAccountModel.Add(AccountModel);

            AccountModel = new AccountModel
            {
                Id = 3,
                Name = Faker.Name.FullName(),
                Status = GetStatusTypeRandom(),
                ParentAccountId = parentModel.Id,
                ParentAccount = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListAccountModel.Add(AccountModel);

            var categoryRequestDto = new CategoryRequestDto
            {
                Id = 1
            };

            var parentAccountRequestDto = new AccountRequestDto
            {
                Id = 1
            };

            AccountRequestDto = new AccountRequestDto
            {
                Name = AccountModel.Name,
                Status = (int)AccountModel.Status,
                ParentAccount = parentAccountRequestDto,
                Category = categoryRequestDto,
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
    }
}