using System.Net;
using System.Web.Http;

namespace Domino.Application
{
    public class DominoServices: IDominoServices
    {
        private readonly int TWO = 2;
        private readonly int SIX = 6;
        public DominoServices()
        {

        }

        public  Task<List<int[]>> GetOrderDominoesAsync(List<int[]> fichas)
        {
            if (fichas == null || fichas.Any(x => x.Length != TWO)  || fichas.Count < TWO || fichas.Count > SIX)
            {
                return Task.FromResult(new List<int[]>());
            }


            for (int i = 0; i < fichas.Count; i++)
            {
                for (int j = i + 1; j < fichas.Count; j++)
                {
                    if (fichas[i][1] == fichas[j][0])
                    {
                        var temp = fichas[j];
                        fichas[j] = fichas[i + 1];
                        fichas[i + 1] = temp;
                        break;
                    }
                    else
                    {
                        if (fichas[j][1] == fichas[i][1])
                        {
                            var temp = new int[] { fichas[j][1], fichas[j][0] };
                            fichas[j] = fichas[i + 1];
                            fichas[i + 1] = temp;
                            break;
                        }
                    }
                }
            }

            if (fichas[0][0] != fichas[fichas.Count - 1][1])
            {
                return Task.FromResult(new List<int[]>());
            }

            return Task.FromResult(fichas); 
        }
        
    }
}
