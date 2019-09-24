using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class Auxiliar
    {

        // Functie reutrn clock de pe server in view
        public static string GetClock()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        // Functie convertire din string in dateTime fomrat
        public static DateTime ReturnareDataFromString(string dateToParse)
        {

            DateTime parsedDate = DateTime.ParseExact(dateToParse,
                                                      "dd.MM.yyyy HH:mm:ss",
                                                      CultureInfo.InvariantCulture);
            return parsedDate;
        }

        // Functie verificare data este din ziua de azi
        public static bool IsCurrentDay(DateTime data)
        {
            if (data.Day == DateTime.Now.Day) return true;
            return false;
        }

        // Functie verificare data este din luna curenta
        public static bool IsCurrentMonth(DateTime data)
        {
            if (data.Month == DateTime.Now.Month) return true;
            return false;
        }

        // Functie verificare data cuprinse intre 2 date (format string) 
        public static bool IsDateBetween(string dataItemString, string dataFromString, string dataToString)
        {
            // Convert string data received from View to DateTime format
            DateTime dataItem = Auxiliar.ReturnareDataFromString(dataItemString);
            DateTime dataFrom = Auxiliar.ReturnareDataFromString(dataFromString + " 00:00:00");
            DateTime dataTo = Auxiliar.ReturnareDataFromString(dataToString + " 00:00:00");
            if (dataItem.CompareTo(dataFrom) >= 0)
            {
                if (dataItem.CompareTo(dataTo) <= 0)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
