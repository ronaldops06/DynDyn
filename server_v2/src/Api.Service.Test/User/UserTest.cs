using Domain.Helpers;
using Domain.Models;

namespace Api.Service.Test.User
{
    public class UserTest
    {
        public static string UserName { get; set; }
        public static string UserLogin { get; set; }
        public static string UserRole { get; set; }
        public static string UserNameUpdated { get; set; }
        public static string UserLoginUpdated { get; set; }
        public static string UserRoleUpdated { get; set; }
        public static int UserId { get; set; }

        public List<UserModel> listUserModel = new List<UserModel>();
        public UserModel userModel;
        public UserModel userModelResult;
        public UserModel userModelUpdate;
        public UserModel userModelUpdateResult;
        public PageParams pageParams;
        public PageList<UserModel> pageListResult;

        public UserTest()
        {
            UserId = 0;
            UserName = Faker.Name.FullName();
            UserLogin = Faker.Internet.Email();
            UserRole = "";
            UserNameUpdated = Faker.Name.FullName();
            UserLoginUpdated = Faker.Internet.Email();

            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= 10; i++)
            {
                var dto = new UserModel()
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Login = Faker.Internet.Email(),
                    Role = "",
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listUserModel.Add(dto);
            }

            pageListResult = new PageList<UserModel>(listUserModel.Skip(0).Take(4).ToList(), 5, pageParams.PageNumber, pageParams.PageSize);

            userModel = new UserModel
            {
                Id = UserId,
                Name = UserName,
                Login = UserLogin,
                Role = UserRole
            };

            userModelResult = new UserModel
            {
                Id = UserId,
                Name = UserName,
                Login = UserLogin,
                Role = UserRole,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            userModelUpdate = new UserModel
            {
                Id = UserId,
                Name = UserNameUpdated,
                Login = UserLoginUpdated,
                Role = UserRoleUpdated
            };

            userModelUpdateResult = new UserModel
            {
                Id = UserId,
                Name = UserNameUpdated,
                Login = UserLoginUpdated,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
        }
    }
}
