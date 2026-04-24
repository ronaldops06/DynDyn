using System.Net;
using Api.Domain.Dtos.{{model}};
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
    public class {{model}}Controller: ControllerBase
    {
        private readonly I{{model}}Service _service; 
        private IMapper _mapper;

        public {{model}}Controller(I{{model}}Service service, IMapper mapper)
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

                var resultDto = _mapper.Map<{{model}}ResponseDto>(model);

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

                var resultDto = _mapper.Map<List<{{model}}ResponseDto>>(pageList);

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
        public async Task<IActionResult> Post([FromBody] {{model}}RequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultDto = new {{model}}ResponseDto();

            try
            {
                var model = _mapper.Map<{{model}}Model>(requestDto);

                model = await _service.Post(model);
                resultDto = _mapper.Map<{{model}}ResponseDto>(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Info<{{model}}Controller>(ex.Message);
                return BadRequest(ex.Message);
            }

            return Created($"/api/{{alias}}/{resultDto.Id}", resultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] {{model}}RequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultDto = new {{model}}ResponseDto();

            try
            {
                var model = _mapper.Map<{{model}}Model>(requestDto);

                var result = await _service.Put(model);
                resultDto = _mapper.Map<{{model}}ResponseDto>(result);
}
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/{{alias}}/{resultDto.Id}", resultDto);
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