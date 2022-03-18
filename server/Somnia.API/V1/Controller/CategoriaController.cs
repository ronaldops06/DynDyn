using AutoMapper;
using Somnia.API.Data;
using Somnia.API.Helpers;
using Somnia.API.Models;
using Somnia.API.V1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Somnia.API.V1.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriaController : ControllerBase
    {
        public readonly IRepository _repository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public CategoriaController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            var categoria =  _repository.GetCategoriaById(id);
            if (categoria == null)
            {
                BadRequest("Categoria não encontrada");
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            var categorias = await _repository.GetAllCategoriasAsync(pageParams);
            var categoriasResult = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            Response.AddPagination(categorias.CurrentPage, categorias.PageSize, categorias.TotalCount, categorias.TotalPages);
            return Ok(categoriasResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Post(CategoriaRegistrarDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _repository.Add(categoria);
            if (_repository.SaveChanges())
            {

                return Created($"/api/categoria/{categoria.ID}", _mapper.Map<CategoriaDTO>(categoria));
            }

            return BadRequest("Categoria não cadastrada.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoriaDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, CategoriaRegistrarDTO categoriaDto)
        {
            var categoria = _repository.GetCategoriaById(id);
            if (categoria == null)
            {
                return BadRequest("Categoria não encontrada.");
            }

            _mapper.Map(categoriaDto, categoria);

            _repository.Update(categoria);
            if (_repository.SaveChanges())
            {
                return Created($"/api/categoria/{categoriaDto.ID}", _mapper.Map<CategoriaDTO>(categoria));
            }

            return BadRequest("Categoria não atualizada.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var categoria = _repository.GetCategoriaById(id);
            if (categoria == null)
            {
                return BadRequest("Categoria não encontrada.");
            }

            _repository.Delete(categoria);
            if (_repository.SaveChanges())
            {
                return Ok("Categoria deletada");
            }

            return BadRequest("Categoria não deletada");
        }
    }
}
