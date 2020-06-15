using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipForms
{
    class Metodos
    {
        public int CantidadBarcos = 0;
        public int CantidadIslas = 0;
        public int CantidadFosas = 0;
        public int ValorParaMapa()
        {
            Random random = new Random();
            int opcion = random.Next(1, 5);
            switch (opcion)
            {
                case 1:
                    return -20;
                case 2:
                    if (CantidadBarcos < 3)
                    {
                        CantidadBarcos++;
                        return 30;
                    }
                    else return -20;
                case 3:
                    if (CantidadIslas < 2)
                    {
                        CantidadIslas++;
                        return 50;
                    }
                    else return -20;
                case 4:
                    if (CantidadFosas < 1)
                    {
                        CantidadFosas++;
                        return -1;
                    }
                    else if (CantidadIslas < 2)
                    {
                        CantidadIslas++;
                        return 50;
                    } 
                    else return -20;
                default:
                    break;
            }
            return 0;
        }
    }
}
