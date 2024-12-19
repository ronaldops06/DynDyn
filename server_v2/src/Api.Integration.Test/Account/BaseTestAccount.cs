using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.Account
{
    public class BaseTestAccount : BaseIntegration
    {
        protected class CategoryBase
        {
            public int CategoryId { get; set; }
            public string CategoryNome { get; set; }
            public int CategoryTipo { get; set; }
            public int CategoryStatus { get; set; }
        }

        protected class AccountBase
        {
            public int AccountId { get; set; }
            public string AccountName { get; set; }
            public int AccountStatus { get; set; }
            public CategoryBase AccountCategory { get; set; }
            public AccountBase AccountParentAccount { get; set; }
        }

        protected AccountRequestDto AccountRequestDto;
        protected CategoryRequestDto CategoryRequestDto;
        protected AccountRequestDto ParentAccountRequestDto;
        protected AccountBase AccountBaseDto;
        protected PageParams PageParams;

        protected BaseTestAccount()
        {
            PageParams = new PageParams()
            {
                Tipo = 1,
                PageNumber = 1,
                PageSize = 3
            };

            var categoryBase = new CategoryBase
            {
                CategoryId = 2,
                CategoryNome = "Corrente",
                CategoryTipo = (int)CategoryType.Conta,
                CategoryStatus = (int)StatusType.Ativo
            };

            var parentBase = new AccountBase
            {
                AccountId = 1,
                AccountName = "Geral",
                AccountStatus = (int)StatusType.Ativo,
                AccountCategory = categoryBase
            };

            AccountBaseDto = new AccountBase
            {
                AccountId = 2,
                AccountName = "Cash",
                AccountStatus = (int)StatusType.Ativo,
                AccountCategory = categoryBase,
                AccountParentAccount = parentBase
            };

            AccountRequestDto = new AccountRequestDto
            {
                Name = "",
                Status = 3,
                Category = null
            };
        }

        protected void GenerateRequestDto()
        {
            CategoryRequestDto = new CategoryRequestDto()
            {
                Id = AccountBaseDto.AccountCategory.CategoryId,
                Name = AccountBaseDto.AccountCategory.CategoryNome,
                Status = AccountBaseDto.AccountCategory.CategoryStatus,
                Type = AccountBaseDto.AccountCategory.CategoryTipo
            };

            ParentAccountRequestDto = new AccountRequestDto
            {
                Id = AccountBaseDto.AccountParentAccount.AccountId,
                Name = AccountBaseDto.AccountParentAccount.AccountName,
                Status = AccountBaseDto.AccountParentAccount.AccountStatus,
                Category = CategoryRequestDto
            };

            AccountRequestDto = new AccountRequestDto
            {
                Id = AccountBaseDto.AccountId,
                Name = AccountBaseDto.AccountName,
                Status = AccountBaseDto.AccountStatus,
                ParentAccount = ParentAccountRequestDto,
                Category = CategoryRequestDto
            };
        }
    }
}