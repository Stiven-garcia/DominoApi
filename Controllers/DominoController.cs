using Domino.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domino.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class DominoController : ControllerBase
    {
       
        private readonly IDominoServices dominoServices;

        public DominoController(IDominoServices dominoServices)
        {
            this.dominoServices = dominoServices;
        }

        /// <summary>
        /// Ordenar una secuencia de fichas de domino
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     [
        ///        [2,1],
        ///        [2,3],
        ///        [1,3]
        ///     ]
        /// </remarks>
        /// <param name="fichas">conjunto de fichas a ordenar</param>
        /// <returns></returns>
        [HttpPost(Name = "GetOrderDominoes")]
        public async Task<IActionResult> GetOrderDominoes(IEnumerable<int[]> fichas)
        {
            var result = await dominoServices.GetOrderDominoesAsync(fichas.ToList()).ConfigureAwait(false);

            if (result == null || !result.Any())
            {
                return BadRequest("La entrada no posee una secuencia de fichas de domino deseable. Verifique e intente Nuevamente");
            }

            return Ok(result);
        }
    }
}