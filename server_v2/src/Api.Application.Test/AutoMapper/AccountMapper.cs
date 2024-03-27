using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;

namespace Api.Application.Test.AutoMapper
{
    public class AccountMapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var categoryRequestDto = new CategoryRequestDto
            {
                Id = 1,
                Nome = "Corrente",
                Tipo = (int)CategoryType.Conta,
                Status = (int)StatusType.Ativo
            };

            var parentDto = new AccountRequestDto
            {
                Id = 1,
                Name = "Geral",
                Status = (int)StatusType.Ativo,
                Category = categoryRequestDto
            };

            var accountRequestDto = new AccountRequestDto
            {
                Id = 2,
                Name = "Cash",
                Status = (int)StatusType.Ativo,
                ParentAccount = parentDto,
                Category = categoryRequestDto
            };

            //Dto -> Model
            var model = Mapper.Map<AccountModel>(accountRequestDto);
            Assert.Equal(model.Id, accountRequestDto.Id);
            Assert.Equal(model.Name, accountRequestDto.Name);
            Assert.Equal((int)model.Status, accountRequestDto.Status);
            Assert.Equal(model.Category.Id, accountRequestDto.Category.Id);
            Assert.Equal(model.CategoryId, accountRequestDto.Category.Id);
            Assert.Equal(model.ParentAccount.Id, accountRequestDto.ParentAccount.Id);
            Assert.Equal(model.ParentAccountId, accountRequestDto.ParentAccount.Id);

            //Model -> DtoResult
            var accountResponseDto = Mapper.Map<AccountResponseDto>(model);
            Assert.Equal(accountResponseDto.Id, model.Id);
            Assert.Equal(accountResponseDto.Name, model.Name);
            Assert.Equal(accountResponseDto.Status, (int)model.Status);
            Assert.Equal(accountResponseDto.Category.Id, model.Category.Id);
            Assert.Equal(accountResponseDto.ParentAccount.Id, model.ParentAccount.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
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
                Category = categoryModel
            };

            var listModel = new List<AccountModel>();

            for (int i = 1; i <= 5; i++)
            {
                var accountModel = new AccountModel
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Status = StatusType.Ativo,
                    ParentAccountId = parentModel.Id,
                    ParentAccount = parentModel,
                    CategoryId = categoryModel.Id,
                    Category = categoryModel,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow

                };
                listModel.Add(accountModel);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<AccountResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Name, listModel[i].Name);
                Assert.Equal(listDto[i].Status, (int)listModel[i].Status);
                Assert.Equal(listDto[i].ParentAccount.Id, listModel[i].ParentAccount.Id);
                Assert.Equal(listDto[i].Category.Id, listModel[i].Category.Id);
            }

            var pageList = new PageList<AccountModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<AccountResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Name, listModel[i].Name);
                Assert.Equal(listResponseDto[i].Status, (int)listModel[i].Status);
                Assert.Equal(listResponseDto[i].ParentAccount.Id, listModel[i].ParentAccount.Id);
                Assert.Equal(listResponseDto[i].Category.Id, listModel[i].Category.Id);
            }
        }
    }
}