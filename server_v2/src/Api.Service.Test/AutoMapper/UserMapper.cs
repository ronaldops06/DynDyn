using Domain.Dtos.User;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class UserMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var dto = new UserDto
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Login = Faker.Internet.Email(),
                Password = Faker.Lorem.GetFirstWord(),
                Role = "A"
            };

            //Dto -> Model
            var model = Mapper.Map<UserModel>(dto);
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal(model.Login, dto.Login);
            Assert.Equal(model.Password, dto.Password);
            Assert.Equal(model.Role, dto.Role);

            //Model -> Entity
            var entity = Mapper.Map<UserEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Login, model.Login);
            Assert.Equal(entity.Password, model.Password);
            Assert.Equal(entity.Role, model.Role);
            
            //Entity -> Model
            var userModel = Mapper.Map<UserModel>(entity);
            Assert.Equal(userModel.Id, entity.Id);
            Assert.Equal(userModel.Name, entity.Name);
            Assert.Equal(userModel.Login, entity.Login);
            Assert.Equal(userModel.Password, entity.Password);
            Assert.Equal(userModel.Role, entity.Role);

            //Model -> DtoResult
            var userResultDto = Mapper.Map<UserResponseDto>(userModel);
            Assert.Equal(userResultDto.Id, userModel.Id);
            Assert.Equal(userResultDto.Name, userModel.Name);
            Assert.Equal(userResultDto.Login, userModel.Login);
            Assert.Equal(userResultDto.Role, userModel.Role);

            var listEntity = new List<UserEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new UserEntity
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Login = Faker.Internet.Email(),
                    Password = Faker.Lorem.GetFirstWord(),
                    Role = "A",
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<UserModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Name, listEntity[i].Name);
                Assert.Equal(listModel[i].Login, listEntity[i].Login);
                Assert.Equal(listModel[i].Password, listEntity[i].Password);
                Assert.Equal(listModel[i].Role, listEntity[i].Role);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<UserDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Name, listModel[i].Name);
                Assert.Equal(listDto[i].Login, listModel[i].Login);
                Assert.Equal(listDto[i].Password, listModel[i].Password);
                Assert.Equal(listDto[i].Role, listModel[i].Role);
            }


            var pageList = new PageList<UserModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listUserResultDto = Mapper.Map<List<UserResponseDto>>(pageList);
            
            Assert.True(listUserResultDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listUserResultDto[i].Id, listModel[i].Id);
                Assert.Equal(listUserResultDto[i].Name, listModel[i].Name);
                Assert.Equal(listUserResultDto[i].Login, listModel[i].Login);
                Assert.Equal(listUserResultDto[i].Role, listModel[i].Role);
            }
        }
    }
}
