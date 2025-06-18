using AutoMapper;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TransientUserController : ControllerBase
{
    private ITransientUserService _service;
    private IMapper _mapper;

    public TransientUserController(ITransientUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] UserRequestDto userRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var userModel = _mapper.Map<TransientUserModel>(userRequestDto);

            userModel = await _service.Post(userModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok("");
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("UserValidate")]
    public async Task<IActionResult> Post([FromBody] ValidationUserDto validationUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var loginResponseDto = new LoginResponseDto();
        try
        {
            var userModel = await _service.ExecuteVerificationCode(validationUserDto.Login, validationUserDto.VerificationCode);
            loginResponseDto = _mapper.Map<LoginResponseDto>(userModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Created($"/api/user", loginResponseDto);
    }
}