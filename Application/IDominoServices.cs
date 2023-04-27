namespace Domino.Application
{
    public interface IDominoServices
    {
        /// <summary>
        /// Ordena una secuencia de fichas de domino
        /// </summary>
        /// <param name="fichas"></param>
        /// <returns></returns>
        Task<List<int[]>> GetOrderDominoesAsync(List<int[]> fichas);
    }
}
