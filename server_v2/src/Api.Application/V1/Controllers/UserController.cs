using AutoMapper;
using Domain.Dtos.User;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        private IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pageList = await _service.Get(pageParams);

                var usersResultDto = _mapper.Map<List<UserResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(usersResultDto);
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
        public async Task<IActionResult> Post(UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userResultDto = new UserResponseDto();

            try
            {
                var userModel = _mapper.Map<UserModel>(userRequestDto);

                userModel = await _service.Post(userModel);
                userResultDto = _mapper.Map<UserResponseDto>(userModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/user/{userResultDto.Id}", userResultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userResultDto = new UserResponseDto();

            try
            {
                var userModel = _mapper.Map<UserModel>(userRequestDto);

                var result = await _service.Put(userModel);
                userResultDto = _mapper.Map<UserResponseDto>(userModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/user/{userResultDto.Id}", userResultDto);
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
