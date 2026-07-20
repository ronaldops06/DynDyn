using Api.Domain.Enums;

namespace Api.Application.Test.Helpers;

public class AttributeHelper
{
    public static AttributeActionType GetAttributeActionTypeRandom()
    {
        Array values = Enum.GetValues(typeof(AttributeActionType));

        Random random = new Random();
        return (AttributeActionType)values.GetValue(random.Next(values.Length));
    }
    
    public static AttributeDataType GetAttributeDataTypeRandom()
    {
        Array values = Enum.GetValues(typeof(AttributeDataType));

        Random random = new Random();
        return (AttributeDataType)values.GetValue(random.Next(values.Length));
    }
}