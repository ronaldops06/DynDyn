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
    public class ContaController : ControllerBase
    {
        public readonly IRepository _repository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public ContaController(IRepository repository, IMapper mapper)
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
            var conta = _repository.GetContaById(id);
            if (conta == null)
            {
                BadRequest("Conta não encontrada");
            }

            var contaDto = _mapper.Map<ContaDTO>(conta);

            return Ok(contaDto);
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
            var contas = await _repository.GetAllContasAsync(pageParams);
            var contasResult = _mapper.Map<IEnumerable<ContaDTO>>(contas);

            Response.AddPagination(contas.CurrentPage, contas.PageSize, contas.TotalCount, contas.TotalPages);

            return Ok(contasResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Post(ContaRegistrarDTO contaDto)
        {
            var conta = _mapper.Map<Conta>(contaDto);

            _repository.Add(conta);
            _repository.UnchangedParentConta(conta);
            if (_repository.SaveChanges())
            {

                return Created($"/api/conta/{conta.ID}", _mapper.Map<ContaDTO>(conta));
            }

            return BadRequest("Conta não cadastrada.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contaDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, ContaRegistrarDTO contaDto)
        {
            var conta = _repository.GetContaById(id);
            if (conta == null)
            {
                return BadRequest("Conta não encontrada.");
            }

            _mapper.Map(contaDto, conta);

            _repository.Update(conta);
            _repository.UnchangedParentConta(conta);
            if (_repository.SaveChanges())
            {
                return Created($"/api/conta/{contaDto.ID}", _mapper.Map<ContaDTO>(conta));
            }

            return BadRequest("Conta não atualizada.");
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
            var conta = _repository.GetContaById(id);
            if (conta == null)
            {
                return BadRequest("Conta não encontrada.");
            }

            _repository.Delete(conta);
            if (_repository.SaveChanges())
            {
                return Ok("Conta deletada");
            }

            return BadRequest("Conta não deletada");
        }
    }
}
