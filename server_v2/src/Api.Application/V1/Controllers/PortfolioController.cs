using System.Net;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioService _service;
        private IMapper _mapper;

        public PortfolioController(IPortfolioService service, IMapper mapper)
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
                var portfolioModel = await _service.GetById(id);

                var portfoliosResultDto = _mapper.Map<PortfolioResponseDto>(portfolioModel);

                return Ok(portfoliosResultDto);
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

                var portfoliosResultDto = _mapper.Map<List<PortfolioResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(portfoliosResultDto);
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
        public async Task<IActionResult> Post([FromBody] PortfolioRequestDto portfolioRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var portfolioResultDto = new PortfolioResponseDto();

            try
            {
                var portfolioModel = _mapper.Map<PortfolioModel>(portfolioRequestDto);

                portfolioModel = await _service.Post(portfolioModel);
                portfolioResultDto = _mapper.Map<PortfolioResponseDto>(portfolioModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/portfolio/{portfolioResultDto.Id}", portfolioResultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] PortfolioRequestDto portfolioRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var portfolioResultDto = new PortfolioResponseDto();

            try
            {
                var portfolioModel = _mapper.Map<PortfolioModel>(portfolioRequestDto);

                var result = await _service.Put(portfolioModel);
                portfolioResultDto = _mapper.Map<PortfolioResponseDto>(portfolioModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Info<PortfolioController>(ex.Message);
                return BadRequest(ex.Message);
            }

            return Created($"/api/portfolio/{portfolioResultDto.Id}", portfolioResultDto);
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