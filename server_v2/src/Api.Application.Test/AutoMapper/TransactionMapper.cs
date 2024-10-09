using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;

namespace Api.Application.Test.AutoMapper
{
    public class TransactionMapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Dtoos()
        {
            var accountRequestDto = GenerateAccount(2, "Cach");
            var operationRequestDto = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);

            var transactionRequestDto = new TransactionRequestDto()
            {
                Id = 2,
                Value = 500.54,
                Observation = "Pago via pix",
                Consolidated = false,
                Installment = null,
                TotalInstallments = null,
                Account = accountRequestDto,
                Operation = operationRequestDto
            };

            //Dto -> Model
            var model = Mapper.Map<TransactionModel>(transactionRequestDto);
            Assert.Equal(model.Id, transactionRequestDto.Id);
            Assert.Equal(model.Value, transactionRequestDto.Value);
            Assert.Equal(model.Observation, transactionRequestDto.Observation);
            Assert.Equal(model.Installment, transactionRequestDto.Installment);
            Assert.Equal(model.Consolidated, transactionRequestDto.Consolidated ? SituationType.Sim : SituationType.Nao);
            Assert.Equal(model.TotalInstallments, transactionRequestDto.TotalInstallments);
            Assert.Equal(model.AccountId, transactionRequestDto.Account.Id);
            Assert.Equal(model.Account.Id, transactionRequestDto.Account.Id);
            Assert.Equal(model.OperationId, transactionRequestDto.Operation.Id);
            Assert.Equal(model.Operation.Id, transactionRequestDto.Operation.Id);
            Assert.Equal(model.ParentTransactionId, transactionRequestDto.ParentTransaction?.Id);
            Assert.Equal(model.ParentTransaction?.Id, transactionRequestDto.ParentTransaction?.Id);

            //Model -> DtoResult
            var transactionResponseDto = Mapper.Map<TransactionResponseDto>(model);
            Assert.Equal(transactionResponseDto.Id, model.Id);
            Assert.Equal(transactionResponseDto.Value, model.Value);
            Assert.Equal(transactionResponseDto.Observation, model.Observation);
            Assert.Equal(transactionResponseDto.Installment, model.Installment);
            Assert.Equal(transactionResponseDto.Consolidated ? SituationType.Sim : SituationType.Nao, model.Consolidated);
            Assert.Equal(transactionResponseDto.TotalInstallments, model.TotalInstallments);
            Assert.Equal(transactionResponseDto.Account.Id, model.Account.Id);
            Assert.Equal(transactionResponseDto.Operation.Id, model.Operation.Id);
            Assert.Equal(transactionResponseDto.ParentTransaction?.Id, model.ParentTransaction?.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var accountRequestDto = GenerateAccount(2, "Cach");
            var operationRequestDto = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);

            var listRequestDto = new List<TransactionRequestDto>();
            Random random = new Random();

            for (int i = 1; i <= 5; i++)
            {
                var transactionRequestDto = new TransactionRequestDto()
                {
                    Id = i,
                    Value = random.Next(5000),
                    Observation = "Pago via pix",
                    Consolidated = false,
                    Installment = null,
                    TotalInstallments = null,
                    Account = accountRequestDto,
                    Operation = operationRequestDto
                };

                listRequestDto.Add(transactionRequestDto);
            }

            var listModel = Mapper.Map<List<TransactionModel>>(listRequestDto);

            //List<Model> -> List<Dto>
            var listResponseDto = Mapper.Map<List<TransactionResponseDto>>(listModel);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listResponseDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Value, listModel[i].Value);
                Assert.Equal(listResponseDto[i].Observation, listModel[i].Observation);
                Assert.Equal(listResponseDto[i].Installment, listModel[i].Installment);
                Assert.Equal(listResponseDto[i].Consolidated ? SituationType.Sim : SituationType.Nao, listModel[i].Consolidated);
                Assert.Equal(listResponseDto[i].TotalInstallments, listModel[i].TotalInstallments);
                Assert.Equal(listResponseDto[i].Account.Id, listModel[i].Account.Id);
                Assert.Equal(listResponseDto[i].Operation.Id, listModel[i].Operation.Id);
                Assert.Equal(listResponseDto[i].ParentTransaction?.Id, listModel[i].ParentTransaction?.Id);
            }

            var pageList = new PageList<TransactionModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var pageListResponse = Mapper.Map<List<TransactionResponseDto>>(pageList);

            Assert.True(pageListResponse.Count() == listModel.Count());

            for (int i = 0; i < listRequestDto.Count(); i++)
            {
                Assert.Equal(pageListResponse[i].Id, listModel[i].Id);
                Assert.Equal(pageListResponse[i].Value, listModel[i].Value);
                Assert.Equal(pageListResponse[i].Observation, listModel[i].Observation);
                Assert.Equal(pageListResponse[i].Installment, listModel[i].Installment);
                Assert.Equal(pageListResponse[i].Consolidated ? SituationType.Sim : SituationType.Nao, listModel[i].Consolidated);
                Assert.Equal(pageListResponse[i].TotalInstallments, listModel[i].TotalInstallments);
                Assert.Equal(pageListResponse[i].Account.Id, listModel[i].Account.Id);
                Assert.Equal(pageListResponse[i].Operation.Id, listModel[i].Operation.Id);
                Assert.Equal(pageListResponse[i].ParentTransaction?.Id, listModel[i].ParentTransaction?.Id);
            }
        }

        private CategoryRequestDto GenerateCategory(CategoryType type, string name, int id)
        {
            return new CategoryRequestDto()
            {
                Id = id,
                Name = name,
                Type = (int)type,
                Status = (int)StatusType.Ativo,
            };
        }

        private AccountRequestDto GenerateAccount(int id, string name)
        {
            var category = GenerateCategory(CategoryType.Conta, "Corrente", 1);

            AccountRequestDto _parentAccountDto = new AccountRequestDto()
            {
                Id = 1,
                Name = "Geral",
                Status = (int)StatusType.Ativo,
                Category = category
            };

            return new AccountRequestDto()
            {
                Id = id,
                Name = name,
                Status = (int)StatusType.Ativo,
                Category = category,
                ParentAccount = _parentAccountDto
            };
        }

        private OperationRequestDto GenerateOperation(int id, string name, OperationType type)
        {
            var category = GenerateCategory(CategoryType.Operação, "Eletrônicos", 2);

            return new OperationRequestDto()
            {
                Id = id,
                Name = name,
                Recurrent = false,
                Type = (int)type,
                Status = (int)StatusType.Ativo,
                Category = category,
            };
        }
    }
}