using System.Globalization;
using System.Net;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using AutoMapper;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _service;
        private IMapper _mapper;

        public TransactionController(ITransactionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transactionModel = await _service.GetById(id);

                var transactionsResultDto = _mapper.Map<TransactionResponseDto>(transactionModel);

                return Ok(transactionsResultDto);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pageList = await _service.Get(pageParams);

                var transactionsResultDto = _mapper.Map<List<TransactionResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(transactionsResultDto);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("Totais")]
        [Authorize]
        public async Task<IActionResult> GetTotais([FromQuery] PageParams pageParams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transactionTotalModel = await _service.GetTotais(pageParams);

                var transactionTotalResultDto = _mapper.Map<TransactionTotalResponseDto>(transactionTotalModel);

                return Ok(transactionTotalResultDto);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPost("recurring")]
        [Authorize]
        public async Task<IActionResult> PostRecurring([FromQuery] PageParams pageParams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.GenerateRecurringAndInstallmentPayments(pageParams.BaseDate);
                
                return Ok("Processo de geração das transações recorrentes executado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize("Bearer")]
        public async Task<IActionResult> Post([FromBody] TransactionRequestDto transactionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionResultDto = new TransactionResponseDto();

            try
            {
                var transactionModel = _mapper.Map<TransactionModel>(transactionRequestDto);

                transactionModel = await _service.Post(transactionModel);
                transactionResultDto = _mapper.Map<TransactionResponseDto>(transactionModel);
            }
            catch (ArgumentException ex)
            {
                Log.Info<TransactionController>($"Erro no cadastro de transação. Erro: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Info<TransactionController>($"Erro no cadastro de transação. Erro: {ex.Message}");
                return BadRequest(ex.Message);
            }

            return Created($"/api/transaction/{transactionResultDto.Id}", transactionResultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] TransactionRequestDto transactionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionResultDto = new TransactionResponseDto();

            try
            {
                var transactionModel = _mapper.Map<TransactionModel>(transactionRequestDto);

                var result = await _service.Put(transactionModel);
                transactionResultDto = _mapper.Map<TransactionResponseDto>(transactionModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/transaction/{transactionResultDto.Id}", transactionResultDto);
        }

        [HttpDelete("{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _service.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}