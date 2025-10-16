using System.Data;
using Api.Domain.Entities;
using Dapper;
using Worker.Service.Interfaces;
using Worker.Service.Models;

namespace Worker.Service.Data.Repository;

public class TransactionRepository : BaseRepository, ITransactionRepository
{
    public TransactionRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task<IEnumerable<PayableTransactionModel>> SelectPayableTransactionsAsync()
    {
        var transactions = await _connection.QueryAsync<PayableTransactionModel>(
              "SELECT trn.\"Value\" Value" +
              "     , ope.\"Name\" OperationName" +
              "     , dvc.\"NotificationToken\" NotificationToken" +
              "  FROM \"Transaction\" trn" +
              "     , \"Operation\"   ope" +
              "     , \"Device\"      dvc" +
              " WHERE trn.\"OperationId\"  = ope.\"Id\"" +
              "   AND trn.\"UserId\"       = dvc.\"UserId\"" +
              "   AND TO_CHAR(trn.\"DataAlteracao\", 'DD/MM/YYYY') = TO_CHAR(NOW(), 'DD/MM/YYYY')" +
              "   AND trn.\"Consolidated\" = @consolidated" +
              "   AND ope.\"Type\"         = @type" +
              " ORDER BY trn.\"UserId\"",
              new { consolidated = 0, type = 2 }
            );

        return transactions;
    }
}