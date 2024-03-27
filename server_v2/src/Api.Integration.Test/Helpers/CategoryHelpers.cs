using Api.Domain.Enums;

namespace Api.Integration.Test.Helpers
{
    public class CategoryHelpers
    {
        public static CategoryType GetCategoryTypeRandom()
        {
            Array values = Enum.GetValues(typeof(CategoryType));

            Random random = new Random();
            return (CategoryType)values.GetValue(random.Next(values.Length));
        }
    }
}