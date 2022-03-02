using AutoMapper;
using Somnia.API.Data;
using Somnia.API.Services;
using Somnia.API.V1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Somnia.API.Models;
using System.Threading.Tasks;
using Somnia.API.Helpers;
using System.Collections.Generic;

namespace Somnia.API.V1.Controller
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
        /// Autenticação
        /// </summary>
        /// <param name="userAuthDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Auth")]
        public IActionResult Authenticate([FromBody] UserAuthDTO userAuthDto)
        {
            if (string.IsNullOrEmpty(userAuthDto.Login) || string.IsNullOrEmpty(userAuthDto.Password))
            {
                return BadRequest("Usuário e senha devem ser informados.");
            }

            // Recupera o usuário
            var user = _repository.GetUsuarioByUsernamaAndPassword(userAuthDto.Login, userAuthDto.Password);

            // Verifica se o usuário existe
            if (user == null)
                return BadRequest("Usuário ou senha inválidos");

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = token;

            return Ok(userDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("AuthValidate")]
        public IActionResult ValidateToken()
        {
            return Ok("Usuário autenticado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            var users = await _repository.GetAllUsuariosAsync(pageParams);
            var usersResult = _mapper.Map<IEnumerable<UserDTO>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersResult);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="userRegisterDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(UserRegistrarDTO userRegisterDTO)
        {
            var user = _mapper.Map<User>(userRegisterDTO);

            var userAux = _repository.GetUsuarioByLogin(user.Login);

            if (userAux != null)
            {
                return BadRequest("Usuário não disponível.");
            }

            _repository.Add(user);
            if (_repository.SaveChanges())
            {
                var token = TokenService.GenerateToken(user);

                var userDto = _mapper.Map<UserDTO>(user);
                userDto.Token = token;
                return Created($"/api/user/{user.ID}", userDto);
            }

            return BadRequest("Usuário não cadastrado.");
        }

        /// <summary>
        /// Atualiza o usuário
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, UserRegistrarDTO userDto)
        {
            var user = _repository.GetUsuarioById(id);
            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            _mapper.Map(userDto, user);

            _repository.Update(user);
            if (_repository.SaveChanges())
            {
                return Created($"/api/user/{user.ID}", _mapper.Map<UserDTO>(user));
            }

            return BadRequest("Usuário não atualizado.");
        }

        /// <summary>
        /// Excluí o usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var user = _repository.GetUsuarioById(id);
            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            _repository.Delete(user);
            if (_repository.SaveChanges())
            {
                return Ok("Usuário deletado");
            }

            return BadRequest("Usuário não deletado");
        }
    }
}
