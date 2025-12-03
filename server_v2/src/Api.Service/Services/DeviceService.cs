using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;
using Newtonsoft.Json;

namespace Service.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IDeviceRepository _repository;
        private readonly IMapper _mapper;

        public DeviceService(IUserService userService, 
                             IDeviceRepository repository, 
                             INotificationService notificationService,
                             IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _notificationService = notificationService;
            _mapper = mapper;
        }
        
        public async Task<DeviceModel> GetById(int id)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Dispositivo não encontrado.");

            return _mapper.Map<DeviceModel>(entity);
        }
        
        public async Task<PageList<DeviceModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<DeviceModel>>(data.Itens);

            return PageList<DeviceModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<DeviceModel> Post(DeviceModel model)
        {
            var user = await _userService.GetLoggedUser();
            var deviceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PhisicalDeviceId);

            if (deviceEntityAux != null)
                throw new Exception("Dispositivo não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var deviceEntity = _mapper.Map<DeviceEntity>(model);
            _repository.UnchangedParentDevice(deviceEntity);
            deviceEntity = await _repository.InsertAsync(deviceEntity);

            model = _mapper.Map<DeviceModel>(deviceEntity);

            return model;
        }

        public async Task<DeviceModel> Put(DeviceModel model)
        {
            var user = await _userService.GetLoggedUser();
            var deviceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PhisicalDeviceId);

            if (deviceEntityAux != null && model.Id != deviceEntityAux.Id)
                throw new Exception("Dispositivo não disponível.");

            deviceEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (deviceEntityAux == null)
                throw new Exception("Dispositivo não encontrado.");
            
            model.User = user;
            model.UserId = user.Id;
            var deviceEntity = _mapper.Map<DeviceEntity>(model);
            _repository.UnchangedParentDevice(deviceEntity);
            deviceEntity = await _repository.UpdateAsync(deviceEntity);

            return _mapper.Map<DeviceModel>(deviceEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetLoggedUser();
            var operationEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (operationEntityAux == null)
                throw new Exception("Dispositivo não encontrado.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<DeviceModel> ExecuteSaveDevice(DeviceModel model)
        {
            var user = await _userService.GetLoggedUser();
            var deviceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PhisicalDeviceId);

            model.User = user;
            model.UserId = user.Id;
            var deviceEntity = _mapper.Map<DeviceEntity>(model);
            _repository.UnchangedParentDevice(deviceEntity);

            if (deviceEntityAux == null)
            {
                deviceEntity = await _repository.InsertAsync(deviceEntity);
            }
            else
            {
                deviceEntity.Id = deviceEntityAux.Id;
                deviceEntity = await _repository.UpdateAsync(deviceEntity);
            }

            return _mapper.Map<DeviceModel>(deviceEntity);
        }
        
    }
}