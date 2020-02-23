using Infraestructura;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] User user)
        {
            return await Task.Run(() =>
            {
                var result = userRepository.List(_user =>
                            (user.Id == 0 || _user.Id == user.Id)
                            &&
                            (user.name == null || _user.Nombre.ToUpper().Equals(_user.Nombre.ToUpper())));
                return Ok(result);
            });
        }
    }
}