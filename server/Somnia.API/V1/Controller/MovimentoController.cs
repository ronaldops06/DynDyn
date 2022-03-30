using AutoMapper;
using Somnia.API.Data;
using Somnia.API.Helpers;
using Somnia.API.Models;
using Somnia.API.V1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Somnia.API.Models.Enums;
using System;

namespace Somnia.API.V1.Controller
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
        [HttpGet("Totais")]
        [Authorize]
        public async Task<IActionResult> GetTotais([FromQuery] PageParams pageParams)
        {
            var totaisMovimentos = await _repository.GetTotaisMovimentosAsync(pageParams);

            var totaisMovimentosDTO = ConvertGroupTotaisInDTO(totaisMovimentos);

            return Ok(totaisMovimentosDTO);
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
            try
            {
                var movimento = _mapper.Map<Movimento>(movimentoDto);

                ValidaTransferencia(movimento);

                OperacaoNotExists(movimento);
                
                movimento.Operacao = null;

                _repository.Add(movimento);
                _repository.UnchangedParentMovimento(movimento);
                if (_repository.SaveChanges())
                {
                    ValidaMovimentoParcelado(movimento);
                    return Created($"/api/movimento/{movimento.ID}", _mapper.Map<MovimentoDTO>(movimento));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            try
            {
                var movimento = _repository.GetMovimentoById(id);
                if (movimento == null)
                {
                    return BadRequest("Movimento não encontrado.");
                }
                
                SubstituiValoresNaoEditaveis(movimento, movimentoDto);

                _mapper.Map(movimentoDto, movimento);
                
                ValidaTransferencia(movimento);

                OperacaoNotExists(movimento);

                movimento.Operacao = null;

                _repository.Update(movimento);
                _repository.UnchangedParentMovimento(movimento);
                if (_repository.SaveChanges())
                {
                    return Created($"/api/movimento/{movimentoDto.ID}", _mapper.Map<MovimentoDTO>(movimento));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public IActionResult Delete(int id, [FromQuery] PageParams pageParams)
        {
            var movimento = _repository.GetMovimentoById(id);
            if (movimento == null)
            {
                return BadRequest("Movimento não encontrado.");
            }

            movimento.Operacao = null;
            movimento.ContaDestino = null;

            _repository.Delete(movimento);
            if (_repository.SaveChanges())
            {
                ValidaExclusaoMovimentoParcelado(pageParams);
                return Ok("Movimento deletado");
            }

            return BadRequest("Movimento não deletado");
        }

        private TotaisMovimentosDTO ConvertGroupTotaisInDTO(Dictionary<OperacaoTipo, double> totaisMovimentos)
        {
            TotaisMovimentosDTO totaisMovimentosDTO = new TotaisMovimentosDTO();

            foreach (var total in totaisMovimentos)
            {
                switch (total.Key)
                {
                    case OperacaoTipo.Credito:
                        totaisMovimentosDTO.Credito = total.Value;
                        break;
                    case OperacaoTipo.Debito:
                        totaisMovimentosDTO.Debito = total.Value;
                        break;
                    case OperacaoTipo.Transferencia:
                        totaisMovimentosDTO.Transferencia = total.Value;
                        break;
                }
            }

            return totaisMovimentosDTO;
        }

        private void ValidaTransferencia(Movimento movimento)
        {
            if (movimento.Operacao.Tipo == OperacaoTipo.Transferencia)
            {
                ExistsContaDestino(movimento.ContaDestinoID);

                IsContasDiferentes(movimento.ContaID, movimento.ContaDestinoID);
            }
        }

        private void ExistsContaDestino(int? contaDestinoID)
        {
            if (contaDestinoID == null || contaDestinoID == 0)
            {
                throw new Exception("A conta de destino deve ser informada.");
            }
        }

        private void IsContasDiferentes(int contaID, int? contaDestinoID)
        {
            if (contaID == contaDestinoID)
            {
                throw new Exception("A conta de destino deve ser diferente da conta origem.");
            }
        }

        private void OperacaoNotExists(Movimento movimento)
        {
            // Podem estar sendo inserido um movimento que ainda não existe a operação, neste caso cria uma nova operação para o movimento.
            if (movimento.OperacaoID == 0)
            {
                ExistsOperacaoByNameAndCategoryAndType(movimento);
            }
        }

        private void ExistsOperacaoByNameAndCategoryAndType(Movimento movimento)
        {
            var operacao = _mapper.Map<Operacao>(movimento.Operacao);

            // Verifica se já não existe uma operação com o mesmo nome, categoria e tipo, se já existir irá somente vinular à que já existe.
            var operacatoAux = _repository.GetOperacaoByNameAndCategoryAndType(operacao.Nome, operacao.CategoriaID, operacao.Tipo);
            if (operacatoAux != null && operacatoAux.ID != 0)
            {
                movimento.OperacaoID = operacatoAux.ID;
            }
            else
            {
                CreateOperacao(movimento, operacao);
            }
        }

        private void CreateOperacao(Movimento movimento, Operacao operacao)
        {
            _repository.Add(operacao);
            _repository.UnchangedParentOperacao(operacao);
            if (_repository.SaveChanges())
            {
                movimento.OperacaoID = operacao.ID;
            }
        }

        private void ValidaMovimentoParcelado(Movimento movimento)
        {
            if (movimento.TotalParcelas > 1)
            {
                for (var i = 2; i <= movimento.TotalParcelas; i++)
                {
                    Movimento movimentoParcela = (Movimento) movimento.Clone();
                    movimentoParcela.ID = 0;
                    movimentoParcela.Parcela = i;
                    movimentoParcela.DataCriacao = movimento.DataCriacao.AddMonths(i-1);
                    movimentoParcela.DataAlteracao = movimento.DataAlteracao.AddMonths(i-1);
                    movimentoParcela.Operacao = null;
                    movimentoParcela.MovimentoPaiID = movimento.ID;

                    _repository.Add(movimentoParcela);
                    _repository.UnchangedParentMovimento(movimentoParcela);
                    if (!_repository.SaveChanges())
                    {
                        throw new Exception($"Não foi possível criar o movimento da parcela {i}.");
                    }
                }

                movimento.Parcela = 1;
                movimento.Operacao = null;

                _repository.Update(movimento);
                _repository.UnchangedParentMovimento(movimento);
                if (!_repository.SaveChanges())
                {
                    throw new Exception($"Não foi possível atualizar o movimento da parcela 1.");
                }
            }
        }

        private async void ValidaExclusaoMovimentoParcelado(PageParams pageParams)
        {
            if (pageParams.MovimentoPaiID != null)
            {
                var movimentos = await _repository.GetAllMovimentosAsync(pageParams);
                foreach (var movimento in movimentos)
                {
                    movimento.Operacao = null;
                    movimento.ContaDestino = null;
                    movimento.Conta = null;

                    _repository.Delete(movimento);
                    if (!_repository.SaveChanges())
                    {
                        throw new Exception($"Não foi possível excluir o movimento da parcela {movimento.Parcela}.");
                    }
                }
            }
        }

        private void SubstituiValoresNaoEditaveis(Movimento movimento, MovimentoRegistrarDTO movimentoDto)
        {
            movimentoDto.Parcela = movimento.Parcela;
            movimentoDto.TotalParcelas = movimento.TotalParcelas;
            movimentoDto.MovimentoPaiID = movimento.MovimentoPaiID;
        }
    }
}
