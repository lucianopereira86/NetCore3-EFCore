using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NetCore3WebAPI.Models;
using Dapper;
using NetCore3WebAPI.Infra.Interface;
using System.Linq;

namespace NetCore3WebAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ConnectionStrings con;
        private readonly IUserRepository repo;
        public UserController(ConnectionStrings c, IUserRepository r)
        {
            con = c;
            repo = r;
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] User vm)
        {
            return await Task.Run(() =>
            {
                var result = repo.List(w =>
                            (vm.id == 0 || w.id == vm.id)
                            &&
                            (vm.name == null || w.name.ToUpper().Equals(w.name.ToUpper())));
                return Ok(result);
                //using (var c = new MySqlConnection(con.MySQL))
                //{
                //    var sql = @"SELECT * FROM user 
                //                WHERE (@id = 0 OR id = @id) 
                //                AND (@name IS NULL OR UPPER(name) = UPPER(@name))";
                //    var query = c.Query<User>(sql, vm, commandTimeout: 30);
                //    return Ok(query);
                //}
            });
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User vm)
        {
            return await Task.Run(() =>
            {
                using (var c = new MySqlConnection(con.MySQL))
                {
                    var sql = @"INSERT INTO user (name) VALUES (@name)";
                    c.Execute(sql, vm, commandTimeout: 30);
                    return Ok();
                }
            });
        }
    }
}