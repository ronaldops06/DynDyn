using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;

namespace Service.Services
{
    public class TrashService : ITrashService
    {
        private readonly ITrashRepository _repository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        
        public TrashService(ITrashRepository trashRepository,
                            IUserService userService,
                            IMapper mapper)
        {
            _repository = trashRepository;
            _userService = userService;
            _mapper = mapper;
        }
            
        public async Task<PageList<TrashModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<TrashModel>>(data.Itens);

            return PageList<TrashModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<TrashModel> Post(TrashModel model)
        {
            var user = await _userService.GetLoggedUser();
            var trashEntityAux = await _repository.SelectByUkAsync(user.Id, model.Reference, model.ReferenceId);

            if (trashEntityAux != null)
                throw new Exception("Registro não disponível.");
            
            model.User = user;
            model.UserId = user.Id;
            var trashEntity = _mapper.Map<TrashEntity>(model);
            _repository.UnchangedParentTrash(trashEntity);
            trashEntity = await _repository.InsertAsync(trashEntity);

            model = _mapper.Map<TrashModel>(trashEntity);

            return model;
        }
    }
}