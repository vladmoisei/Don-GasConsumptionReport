using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class Auxiliar
    {

        // Functie reutrn clock de pe server in view
        public static string GetClock()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
