using Domain.Entities;

namespace Api.Data.Test.Helpers;

public class UserHelper
{
    public static UserEntity GetLoggedUserFake()
    {
        return new UserEntity()
        {
            Login = Faker.Internet.Email(),
            Password = "123456",
            Name = "User Test",
            Role = "Admin"
        };
    }
}