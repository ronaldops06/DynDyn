using Api.Domain.Dtos.Device;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DeviceController : ControllerBase
{
    IDeviceService _service;
    private IMapper _mapper;

    public DeviceController(IDeviceService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Authorize]
    [Route("RegisterDevice")]
    public async Task<IActionResult> Post([FromBody] DeviceRequestDto deviceRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var deviceResponseDto = new DeviceResponseDto();
        try
        {
            var deviceModel = _mapper.Map<DeviceModel>(deviceRequestDto);
            deviceModel = await _service.ExecuteSaveDevice(deviceModel);
            
            deviceResponseDto = _mapper.Map<DeviceResponseDto>(deviceModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Created($"/api/device/{deviceResponseDto.Id}", deviceResponseDto);
    }
}