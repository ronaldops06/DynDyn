using Api.Domain.Enums;

namespace Api.Data.Test.Helpers
{
    public class CategoryHelper
    {
        public static CategoryType GetCategoryTypeRandom()
        {
            Array values = Enum.GetValues(typeof(CategoryType));

            Random random = new Random();
            return (CategoryType)values.GetValue(random.Next(values.Length));
        }
    }
}