using Api.Domain.Enums;

namespace Api.Service.Test.Helpers
{
    public class BaseHelper
    {
        public static StatusType GetStatusTypeRandom()
        {
            Array values = Enum.GetValues(typeof(StatusType));

            Random random = new Random();
            return (StatusType)values.GetValue(random.Next(values.Length));
        }
    }
}