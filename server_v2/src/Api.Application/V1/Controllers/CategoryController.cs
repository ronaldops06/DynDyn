using System.Net;
using Api.Domain.Dtos.Category;
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
    public class CategoryController : ControllerBase
    {
        private ICategoryService _service;
        private IMapper _mapper;

        public CategoryController(ICategoryService service, IMapper mapper)
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
                var categoryModel = await _service.GetById(id);

                var categorysResultDto = _mapper.Map<CategoryResponseDto>(categoryModel);

                return Ok(categorysResultDto);
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

                var categorysResultDto = _mapper.Map<List<CategoryResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(categorysResultDto);
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
        public async Task<IActionResult> Post([FromBody] CategoryRequestDto categoryRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryResultDto = new CategoryResponseDto();

            try
            {
                var categoryModel = _mapper.Map<CategoryModel>(categoryRequestDto);

                categoryModel = await _service.Post(categoryModel);
                categoryResultDto = _mapper.Map<CategoryResponseDto>(categoryModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/category/{categoryResultDto.Id}", categoryResultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] CategoryRequestDto categoryRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryResultDto = new CategoryResponseDto();

            try
            {
                var categoryModel = _mapper.Map<CategoryModel>(categoryRequestDto);

                var result = await _service.Put(categoryModel);
                categoryResultDto = _mapper.Map<CategoryResponseDto>(categoryModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/category/{categoryResultDto.Id}", categoryResultDto);
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