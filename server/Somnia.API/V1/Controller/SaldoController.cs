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
    public class SaldoController : ControllerBase
    {
        public readonly IRepository _repository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public SaldoController(IRepository repository, IMapper mapper)
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
            var saldo = _repository.GetSaldoById(id);
            if (saldo == null)
            {
                BadRequest("Saldo não encontrado");
            }

            var saldoDTO = _mapper.Map<SaldoDTO>(saldo);

            return Ok(saldoDTO);
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
            var saldos = await _repository.GetAllSaldosAsync(pageParams);
            var saldosResult = _mapper.Map<IEnumerable<SaldoDTO>>(saldos);

            Response.AddPagination(saldos.CurrentPage, saldos.PageSize, saldos.TotalCount, saldos.TotalPages);

            return Ok(saldosResult);
        }

        /// <summary>
        /// Cria um novo saldo
        /// </summary>
        /// <param name="saldoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Post(SaldoRegistrarDTO saldoDto)
        {
            var saldo = _mapper.Map<Saldo>(saldoDto);

            _repository.Add(saldo);
            _repository.UnchangedParentSaldo(saldo);
            if (_repository.SaveChanges())
            {
                return Created($"/api/saldo/{saldo.ID}", _mapper.Map<SaldoDTO>(saldo));
            }

            return BadRequest("Saldo não cadastrado.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saldoDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, SaldoRegistrarDTO saldoDto)
        {
            var saldo = _repository.GetSaldoById(id);
            if (saldo == null)
            {
                return BadRequest("Saldo não encontrado.");
            }

            _mapper.Map(saldoDto, saldo);

            _repository.Update(saldo);
            _repository.UnchangedParentSaldo(saldo);
            if (_repository.SaveChanges())
            {
                return Created($"/api/saldo/{saldoDto.ID}", _mapper.Map<SaldoDTO>(saldo));
            }

            return BadRequest("Saldo não atualizado.");
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
            var saldo = _repository.GetSaldoById(id);
            if (saldo == null)
            {
                return BadRequest("Saldo não encontrado.");
            }

            _repository.Delete(saldo);
            if (_repository.SaveChanges())
            {
                return Ok("Saldo deletado");
            }

            return BadRequest("Saldo não deletado");
        }
    }
}
