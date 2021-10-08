using AutoMapper;
using DynDyn.API.Data;
using DynDyn.API.Services;
using DynDyn.API.V1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynDyn.API.V1.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IRepository _repository;
        public readonly IMapper _mapper;

        public UserController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAuthDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] UserAuthDTO userAuthDto)
        {
            // Recupera o usuário
            var user = _repository.GetUsuarioByUsernamaAndPassword(userAuthDto.Username, userAuthDto.Password);

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            var contaDto = _mapper.Map<UserDTO>(user);
            contaDto.Token = token;

            return Ok(contaDto);
        }
    }
}
