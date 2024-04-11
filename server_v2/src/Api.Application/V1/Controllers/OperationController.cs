using System.Net;
using Api.Domain.Dtos.Operation;
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
    public class OperationController : ControllerBase
    {
        private IOperationService _service;
        private IMapper _mapper;

        public OperationController(IOperationService service, IMapper mapper)
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
                var operationModel = await _service.GetById(id);

                var operationsResultDto = _mapper.Map<OperationResponseDto>(operationModel);

                return Ok(operationsResultDto);
            }
            catch (ArgumentException ex)
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

                var operationsResultDto = _mapper.Map<List<OperationResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(operationsResultDto);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize("Bearer")]
        public async Task<IActionResult> Post([FromBody] OperationRequestDto operationRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var operationResultDto = new OperationResponseDto();

            try
            {
                var operationModel = _mapper.Map<OperationModel>(operationRequestDto);

                operationModel = await _service.Post(operationModel);
                operationResultDto = _mapper.Map<OperationResponseDto>(operationModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/operation/{operationResultDto.Id}", operationResultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] OperationRequestDto operationRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var operationResultDto = new OperationResponseDto();

            try
            {
                var operationModel = _mapper.Map<OperationModel>(operationRequestDto);

                var result = await _service.Put(operationModel);
                operationResultDto = _mapper.Map<OperationResponseDto>(operationModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/operation/{operationResultDto.Id}", operationResultDto);
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