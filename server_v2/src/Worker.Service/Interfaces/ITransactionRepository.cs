using Api.Domain.Entities;
using Worker.Service.Models;

namespace Worker.Service.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<PayableTransactionModel>> SelectPayableTransactionsAsync();
}