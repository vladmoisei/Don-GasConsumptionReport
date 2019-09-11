using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class Raport
    {
        public static string textBoxListaMailCuptor = "";
        public static string textBoxListaMailGadda = "";

        public static string textBoxOraRaportCuptor = "";
        public static string textBoxOraRaportGadda = "";

        

        // TO DO
        
        /*
         * Functii inregistrare date (index, consum) in SQL Server
         */
        
        // Functie Create Index daily at set hour into SQL Server
        public static void CreateIndexIntoSqlCuptor(string plcName) { }

        // Functie Create Consumption daily into SQL Server
        public static void CreateConsumptionIntoSql(string plcName) { }
        
        /*
         * Functii get date (index, consum) in SQL Server
         */
        
        // Functie GET Index from Sql
        public static string GetIndexFromSql(string plcName) { return ""; }
        // Functie GET Consumption from SQL
        public static string GetConsumptionFromSql(string plcName) { return ""; }

        /*
         * Functii trimitere mail zilnic si lunar
         */

        // Functie Send Email daily with Consumption
        public static void SendEmaildaily(string listaMail) { }
        
        // Functie Send Email monthly with Consumption for all month
        public static void SendEmailmonthly(string listaMail) { }

        /*
         * Functii Get Report into Excel From SQL with Consumption and Index
         */
        // Functie Get Monthly Report intro Excel from Sql with Consumption
        // O sa le fac in Controller
        
    }
}
