namespace Worker.Service.Models;

public class PayableTransactionModel
{
    /// <summary>
    /// Token do broker de push notification
    /// </summary>
    public string NotificationToken { get; set; }
    
    /// <summary>
    /// Nome da operação.
    /// </summary>
    public string OperationName { get; set; }
    
    /// <summary>
    /// Valor do movimento.
    /// </summary>
    public double Value { get; set; }
}