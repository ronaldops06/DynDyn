using AutoMapper;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.V1.Controllers
{
    /// <summary>
    /// Controle responsável por gerenciar a autenticação do usuário
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public LoginController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Método responsável por realizar a autenticação do usuário
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Auth")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto, [FromServices] ILoginService service)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (loginDto == null)
                return BadRequest("Usuário e senha não informados");
            
            var userResultDto = new LoginResponseDto();
            
            try
            {
                var userModel = _mapper.Map<TransientUserModel>(loginDto);
                userModel = await service.GetLoginAsync(userModel);
                userResultDto = _mapper.Map<LoginResponseDto>(userModel);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok(userResultDto);
        }

        /// <summary>
        /// Método responsável por realizar a validação da autenticação do usuário
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("AuthValidate")]
        public IActionResult ValidateToken()
        {
            return Ok("Usuário autenticado");
        }
    }
}
