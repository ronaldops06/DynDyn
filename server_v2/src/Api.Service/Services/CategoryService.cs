using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using AutoMapper;
using Domain.Helpers;

namespace Api.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _repository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repository
                              , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryModel> GetById(int id)
        {
            var entity = await _repository.SelectByIdAsync(id);

            if (entity == null)
                throw new Exception("Categoria não encontrada.");

            return _mapper.Map<CategoryModel>(entity);
        }

        public async Task<PageList<CategoryModel>> Get(PageParams pageParams)
        {
            var data = await _repository.SelectByParamAsync(pageParams);
            var itens = _mapper.Map<List<CategoryModel>>(data.Itens);

            return PageList<CategoryModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<CategoryModel> Post(CategoryModel categoryModel)
        {
            var categoryEntityAux = await _repository.SelectByUkAsync(categoryModel.Name);

            if (categoryEntityAux != null)
                throw new Exception("Categoria não disponível.");

            var categoryEntity = _mapper.Map<CategoryEntity>(categoryModel);
            categoryEntity = await _repository.InsertAsync(categoryEntity);

            categoryModel = _mapper.Map<CategoryModel>(categoryEntity);

            return categoryModel;
        }

        public async Task<CategoryModel> Put(CategoryModel categoryModel)
        {
            var categoryEntityAux = await _repository.SelectByUkAsync(categoryModel.Name);

            if (categoryEntityAux != null && categoryModel.Id != categoryEntityAux.Id)
                throw new Exception("Categoria não disponível.");

            categoryEntityAux = await _repository.SelectByIdAsync(categoryModel.Id);

            if (categoryEntityAux == null)
                throw new Exception("Categoria não encontrada.");

            var categoryEntity = _mapper.Map<CategoryEntity>(categoryModel);
            categoryEntity = await _repository.UpdateAsync(categoryEntity);

            return _mapper.Map<CategoryModel>(categoryEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var categoryEntityAux = await _repository.SelectByIdAsync(id);

            if (categoryEntityAux == null)
                throw new Exception("Categoria não encontrada.");

            return await _repository.DeleteAsync(id);
        }
    }
}