using System.Net;
using Api.Domain.Dtos.Account;
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
    public class AccountController : ControllerBase
    {
        private IAccountService _service;
        private IMapper _mapper;

        public AccountController(IAccountService service, IMapper mapper)
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
                var accountModel = await _service.GetById(id);

                var accountsResultDto = _mapper.Map<AccountResponseDto>(accountModel);

                return Ok(accountsResultDto);
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

                var accountsResultDto = _mapper.Map<List<AccountResponseDto>>(pageList);

                Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

                return Ok(accountsResultDto);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize("Bearer")]
        public async Task<IActionResult> Post([FromBody] AccountRequestDto accountRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accountResultDto = new AccountResponseDto();

            try
            {
                var accountModel = _mapper.Map<AccountModel>(accountRequestDto);

                accountModel = await _service.Post(accountModel);
                accountResultDto = _mapper.Map<AccountResponseDto>(accountModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/account/{accountResultDto.Id}", accountResultDto);
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<IActionResult> Put([FromBody] AccountRequestDto accountRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accountResultDto = new AccountResponseDto();

            try
            {
                var accountModel = _mapper.Map<AccountModel>(accountRequestDto);

                var result = await _service.Put(accountModel);
                accountResultDto = _mapper.Map<AccountResponseDto>(accountModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"/api/account/{accountResultDto.Id}", accountResultDto);
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