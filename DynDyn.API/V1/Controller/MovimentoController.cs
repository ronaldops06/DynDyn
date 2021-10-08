using AutoMapper;
using DynDyn.API.Data;
using DynDyn.API.Helpers;
using DynDyn.API.Models;
using DynDyn.API.V1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynDyn.API.V1.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MovimentoController : ControllerBase
    {
        public readonly IRepository _repository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public MovimentoController(IRepository repository, IMapper mapper)
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
            var movimento = _repository.GetMovimentoById(id);
            if (movimento == null)
            {
                BadRequest("Movimento não encontrado");
            }

            var movimentoDTO = _mapper.Map<MovimentoDTO>(movimento);

            return Ok(movimentoDTO);
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
            var movimentos = await _repository.GetAllMovimentosAsync(pageParams);
            var movimentosResult = _mapper.Map<IEnumerable<MovimentoDTO>>(movimentos);

            Response.AddPagination(movimentos.CurrentPage, movimentos.PageSize, movimentos.TotalCount, movimentos.TotalPages);

            return Ok(movimentosResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movimentoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Post(MovimentoRegistrarDTO movimentoDto)
        {
            var movimento = _mapper.Map<Movimento>(movimentoDto);

            _repository.Add(movimento);
            _repository.UnchangedParentMovimento(movimento);
            if (_repository.SaveChanges())
            {

                return Created($"/api/movimento/{movimento.ID}", _mapper.Map<MovimentoDTO>(movimento));
            }

            return BadRequest("Movimento não cadastrado.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movimentoDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, MovimentoRegistrarDTO movimentoDto)
        {
            var movimento = _repository.GetMovimentoById(id);
            if (movimento == null)
            {
                return BadRequest("Movimento não encontrado.");
            }

            _mapper.Map(movimentoDto, movimento);

            _repository.Update(movimento);
            _repository.UnchangedParentMovimento(movimento);
            if (_repository.SaveChanges())
            {
                return Created($"/api/movimento/{movimentoDto.ID}", _mapper.Map<MovimentoDTO>(movimento));
            }

            return BadRequest("Movimento não atualizado.");
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
            var movimento = _repository.GetMovimentoById(id);
            if (movimento == null)
            {
                return BadRequest("Movimento não encontrado.");
            }

            _repository.Delete(movimento);
            if (_repository.SaveChanges())
            {
                return Ok("Movimento deletado");
            }

            return BadRequest("Movimento não deletado");
        }
    }
}
