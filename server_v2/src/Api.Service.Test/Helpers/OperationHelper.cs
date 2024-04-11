using Api.Domain.Enums;

namespace Api.Service.Test.Helpers
{
    public class OperationHelper
    {
        public static OperationType GetOperationTypeRandom()
        {
            Array values = Enum.GetValues(typeof(OperationType));

            Random random = new Random();
            return (OperationType)values.GetValue(random.Next(values.Length));
        }
    }
}