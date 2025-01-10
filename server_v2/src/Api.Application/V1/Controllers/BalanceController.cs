using System.Net;
using Api.Domain.Dtos.Balance;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BalanceController : ControllerBase
{
    private IBalanceService _service;
    private IMapper _mapper;

    public BalanceController(IBalanceService service, IMapper mapper)
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
            var balanceModel = await _service.GetById(id);

            var balancesResultDto = _mapper.Map<BalanceResponseDto>(balanceModel);

            return Ok(balancesResultDto);
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

            var balancesResultDto = _mapper.Map<List<BalanceResponseDto>>(pageList);

            Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

            return Ok(balancesResultDto);
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
    public async Task<IActionResult> Post([FromBody] BalanceRequestDto balanceRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var balanceResultDto = new BalanceResponseDto();

        try
        {
            var balanceModel = _mapper.Map<BalanceModel>(balanceRequestDto);

            balanceModel = await _service.Post(balanceModel);
            balanceResultDto = _mapper.Map<BalanceResponseDto>(balanceModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Created($"/api/balance/{balanceResultDto.Id}", balanceResultDto);
    }

    [HttpPut]
    [Authorize("Bearer")]
    public async Task<IActionResult> Put([FromBody] BalanceRequestDto balanceRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var balanceResultDto = new BalanceResponseDto();

        try
        {
            var balanceModel = _mapper.Map<BalanceModel>(balanceRequestDto);

            var result = await _service.Put(balanceModel);
            balanceResultDto = _mapper.Map<BalanceResponseDto>(balanceModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Created($"/api/balance/{balanceResultDto.Id}", balanceResultDto);
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