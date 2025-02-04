using Api.Domain.Enums;
using Domain.Models;

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
        
        public static UserModel GetLoggedUserFake()
        {
            return new UserModel()
            {
                Id = 1,
                Login = "teste@gmail.com",
                Password = "123456",
                Name = "User Test",
                Role = "Admin"
            };
        }
    }
}