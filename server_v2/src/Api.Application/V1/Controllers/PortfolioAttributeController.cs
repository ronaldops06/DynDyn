using System.Net;
using Api.Domain.Dtos.PortfolioAttribute;
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
    public class PortfolioAttributeController: ControllerBase
    {
        private readonly IPortfolioAttributeService _service; 
        private IMapper _mapper;

        public PortfolioAttributeController(IPortfolioAttributeService service, IMapper mapper)
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

                var resultDto = _mapper.Map<PortfolioAttributeResponseDto>(model);

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

                var resultDto = _mapper.Map<List<PortfolioAttributeResponseDto>>(pageList);

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

        [HttpGet("portfolio/{portfolioId}")]
        [Authorize]
        public async Task<IActionResult> GetByPortfolio(int portfolioId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var models = await _service.GetByPortfolioAsync(portfolioId);

                var resultDto = _mapper.Map<List<PortfolioAttributeResponseDto>>(models);

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

        [HttpGet("portfolio/{portfolioId}/attribute/{attributeId}")]
        [Authorize]
        public async Task<IActionResult> GetByPortfolioAndAttribute(int portfolioId, int attributeId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var model = await _service.GetByPortfolioAndAttributeAsync(portfolioId, attributeId);

                var resultDto = _mapper.Map<PortfolioAttributeResponseDto>(model);

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
        public async Task<IActionResult> Post([FromBody] PortfolioAttributeRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultDto = new PortfolioAttributeResponseDto();

            try
            {
                var model = _mapper.Map<PortfolioAttributeModel>(requestDto);

                model = await _service.Post(model);
                resultDto = _mapper.Map<PortfolioAttributeResponseDto>(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Info<PortfolioAttributeController>(ex.Message);
                return BadRequest(ex.Message);
            }

            return Created($"/api/v1/portfolioattribute/{resultDto.Id}", resultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] PortfolioAttributeRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultDto = new PortfolioAttributeResponseDto();

            try
            {
                var model = _mapper.Map<PortfolioAttributeModel>(requestDto);

                var result = await _service.Put(model);
                resultDto = _mapper.Map<PortfolioAttributeResponseDto>(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(resultDto);
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