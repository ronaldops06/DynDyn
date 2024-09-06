using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<ITransactionService>();

            serviceMock.Setup(m => m.Put(It.IsAny<TransactionModel>())).ReturnsAsync(TransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);

            var result = await Controller.Put(TransactionRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as TransactionResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(TransactionRequestDto.Id, resultValue.Id);
            Assert.Equal(TransactionRequestDto.Value, resultValue.Value);
            Assert.Equal(TransactionRequestDto.Observation, resultValue.Observation);
            Assert.Equal(TransactionRequestDto.Installment, resultValue.Installment);
            Assert.Equal(TransactionRequestDto.Consolidated, resultValue.Consolidated);
            Assert.Equal(TransactionRequestDto.TotalInstallments, resultValue.TotalInstallments);
            Assert.Equal(TransactionRequestDto.Account.Id, resultValue.Account.Id);
            Assert.Equal(TransactionRequestDto.Operation.Id, resultValue.Operation.Id);
            Assert.Equal(TransactionRequestDto.ParentTransaction?.Id, resultValue.ParentTransaction?.Id);
        }

    }
}