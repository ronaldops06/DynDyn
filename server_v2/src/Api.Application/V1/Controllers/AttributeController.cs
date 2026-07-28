using System.Net;
using Api.Domain.Dtos.Attribute;
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
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _service; 
        private IMapper _mapper;

        public AttributeController(IAttributeService service, IMapper mapper)
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
                var model = await _service.GetById(id);

                var resultDto = _mapper.Map<AttributeResponseDto>(model);

                return Ok(resultDto);
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

                var resultDto = _mapper.Map<List<AttributeResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(resultDto);
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
        public async Task<IActionResult> Post([FromBody] AttributeRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultDto = new AttributeResponseDto();

            try
            {
                var model = _mapper.Map<AttributeModel>(requestDto);

                model = await _service.Post(model);
                resultDto = _mapper.Map<AttributeResponseDto>(model);
            }
            catch (ArgumentException ex)
            {
                Log.Info<AttributeController>(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Info<AttributeController>(ex.Message);
                return BadRequest(ex.Message);
            }

            return Created($"/api/attribute/{resultDto.Id}", resultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] AttributeRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultDto = new AttributeResponseDto();

            try
            {
                var model = _mapper.Map<AttributeModel>(requestDto);

                var result = await _service.Put(model);
                resultDto = _mapper.Map<AttributeResponseDto>(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/attribute/{resultDto.Id}", resultDto);
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
