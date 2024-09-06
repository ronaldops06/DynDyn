using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestCreate
{
    public class ReturnCreated : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<ITransactionService>();

            serviceMock.Setup(m => m.Post(It.IsAny<TransactionModel>())).ReturnsAsync(TransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(TransactionRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as TransactionResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
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