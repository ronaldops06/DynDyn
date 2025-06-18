using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper;

public class TransientUserMapper : BaseTestService
{
    [Fact(DisplayName = "É possível mapear os modelos")]
    public void Eh_Possivel_Mapear_Os_Modelos()
    {
        var model = new TransientUserModel
        {
            Id = 1,
            Name = Faker.Name.FullName(),
            Login = Faker.Internet.Email(),
            Password = Faker.Lorem.GetFirstWord(),
            VerificationCode = Faker.RandomNumber.Next(100000, 999999),
            ExpirationDate = DateTime.Now.AddMinutes(15)
        };
        
        //Model -> Entity
        var entity = Mapper.Map<TransientUserEntity>(model);
        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.Login, model.Login);
        Assert.Equal(entity.Password, model.Password);
        Assert.Equal(entity.VerificationCode, model.VerificationCode);
        Assert.Equal(entity.ExpirationDate, model.ExpirationDate);
            
        //Entity -> Model
        var transientUserModel = Mapper.Map<TransientUserModel>(entity);
        Assert.Equal(transientUserModel.Id, entity.Id);
        Assert.Equal(transientUserModel.Name, entity.Name);
        Assert.Equal(transientUserModel.Login, entity.Login);
        Assert.Equal(transientUserModel.Password, entity.Password);
        Assert.Equal(transientUserModel.VerificationCode, entity.VerificationCode);
        Assert.Equal(transientUserModel.ExpirationDate, entity.ExpirationDate);
        
        //TrasientUserModel -> UserModel
        var userModel = Mapper.Map<UserModel>(model);
        Assert.Equal(userModel.Id, model.Id);
        Assert.Equal(userModel.Name, model.Name);
        Assert.Equal(userModel.Login, model.Login);
        Assert.Equal(userModel.Password, model.Password);
        
        //UserModel -> TransientUserEntity
        var transientUserEntity = Mapper.Map<TransientUserEntity>(userModel);
        Assert.Equal(transientUserEntity.Id, userModel.Id);
        Assert.Equal(transientUserEntity.Name, userModel.Name);
        Assert.Equal(transientUserEntity.Login, userModel.Login);
        Assert.Equal(transientUserEntity.Password, userModel.Password);
        
        //UserEntity -> TrasientUserModel
        var userEntity = Mapper.Map<UserEntity>(userModel);
        transientUserModel = Mapper.Map<TransientUserModel>(userEntity);
        Assert.Equal(transientUserModel.Id, userModel.Id);
        Assert.Equal(transientUserModel.Name, userModel.Name);
        Assert.Equal(transientUserModel.Login, userModel.Login);
        Assert.Equal(transientUserModel.Password, userModel.Password);
    }
}