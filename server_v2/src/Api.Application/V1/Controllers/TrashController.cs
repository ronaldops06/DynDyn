using System.Net;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Maintenance;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TrashController : ControllerBase
{
    private ITrashService _service;
    private IMapper _mapper;

    public TrashController(ITrashService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
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

            var trashResultDto = _mapper.Map<List<TrashResponseDto>>(pageList);

            Response.AddPagination(pageList.CurrentPage, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);

            return Ok(trashResultDto);
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
}