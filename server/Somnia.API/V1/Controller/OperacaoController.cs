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
    public class OperacaoController : ControllerBase
    {
        public readonly IRepository _repository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public OperacaoController(IRepository repository, IMapper mapper)
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
            var operacao = _repository.GetOperacaoById(id);
            if (operacao == null)
            {
                BadRequest("Operação não encontrado");
            }

            var operacaoDTO = _mapper.Map<OperacaoDTO>(operacao);

            return Ok(operacaoDTO);
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
            var operacaos = await _repository.GetAllOperacoesAsync(pageParams);
            var operacaosResult = _mapper.Map<IEnumerable<OperacaoDTO>>(operacaos);

            Response.AddPagination(operacaos.CurrentPage, operacaos.PageSize, operacaos.TotalCount, operacaos.TotalPages);

            return Ok(operacaosResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operacaoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Post(OperacaoRegistrarDTO operacaoDto)
        {
            var operacao = _mapper.Map<Operacao>(operacaoDto);

            _repository.Add(operacao);
            _repository.UnchangedParentOperacao(operacao);
            if (_repository.SaveChanges())
            {

                return Created($"/api/operacao/{operacao.ID}", _mapper.Map<OperacaoDTO>(operacao));
            }

            return BadRequest("Operação não cadastrada.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operacaoDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, OperacaoRegistrarDTO operacaoDto)
        {
            var operacao = _repository.GetOperacaoById(id);
            if (operacao == null)
            {
                return BadRequest("Operação não encontrada.");
            }

            _mapper.Map(operacaoDto, operacao);

            _repository.Update(operacao);
            _repository.UnchangedParentOperacao(operacao);
            if (_repository.SaveChanges())
            {
                return Created($"/api/operacao/{operacaoDto.ID}", _mapper.Map<OperacaoDTO>(operacao));
            }

            return BadRequest("Operação não atualizada.");
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
            var operacao = _repository.GetOperacaoById(id);
            if (operacao == null)
            {
                return BadRequest("Operação não encontrada.");
            }

            _repository.Delete(operacao);
            if (_repository.SaveChanges())
            {
                return Ok("Operação deletada");
            }

            return BadRequest("Operação não deletada");
        }
    }
}
