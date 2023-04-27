using Domino.Application;
using Domino.Application.Request;
using Domino.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domino.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
       
        private readonly IDominoServices dominoServices;

        public AccountController(IDominoServices dominoServices)
        {
            this.dominoServices = dominoServices;
        }

        /// <summary>
        /// Metodo de autenticacion
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(Name = "Loggin")]
        public IActionResult Loggin(UserRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = Utilities.User.Users.Contains(new KeyValuePair<string, string>(user.UserName, user.Password));

            if (exists)
            {
                var token = TokenGenerator.GenerateTokenJwt(user.UserName);
                return Ok(token);
            }
            else
            {
                return Unauthorized("Usuario o contraseña incorrecta");
            }
           
            
        }
    }
}