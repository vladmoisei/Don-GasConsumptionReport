using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class Raport
    {
        public static string ListaMailCuptor = "";
        public static string ListaMailGadda = "";

        public static string OraRaportCuptor = "";
        public static string OraRaportGadda = "";

        // Index GAz
        public static uint IndexCuptor { get; set; } = 0;
        public static uint IndexGaddaF2 { get; set; } = 0;
        public static uint IndexGaddaF4 { get; set; } = 0;

        // Valori consum gaz
        public static int ValoareConsumGazCuptor { get; set; }
        public static int ValoareConsumGazGaddaF2 { get; set; }
        public static int ValoareConsumGazGaddaF4 { get; set; }
        // TO DO

        /*
         * Functii inregistrare date (index, consum) in SQL Server
         */


        // Functie Creare Index Object pentru salvare in SQL in functie de nume plc
        public static IndexModel GetIndexModelObject(string numePlc, uint valoareIndex)
        {
            IndexModel indexModel = new IndexModel { Data = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"), PlcName = numePlc, IndexValue =  (int)valoareIndex};
            return indexModel;
        }

        // Functie salvare in database SQL index gaz model
        public static void AddToSqlIndex(RaportareDbContext context, IndexModel indexModel)
        {
            context.Add(indexModel);
            context.SaveChanges();
        }
        // Functie verificare ora raport
        public static bool VerificareOraRaport(string ora, RaportareDbContext context)
        {
            // Se verifica daca este ora raport
            if (DateTime.Now.ToString("HH:mm:ss") == ora)
            {
                // Refresh values plc
                PlcService.RefreshValuesListaPlc();

                foreach (PlcObjectModel plc in PlcService.ListaPlc)
                {
                    // Salvare valoare index citita in varabile statice raport
                    switch (plc.PlcName)
                    {
                        case "PlcCuptor":
                            IndexCuptor = plc.ValoareIndexGaz;
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            break;
                        case "PlcGaddaF2":
                            IndexGaddaF2 = plc.ValoareIndexGaz; //Dint
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            break;
                        case "PlcGaddaF4":
                            IndexGaddaF4 = plc.ValoareIndexGaz; //Dint
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            break;
                        default:
                            break;
                    }

                    // Creare IndexModelObject pentru a salva in SQL Stabase

                }




                return true;
            }
            return false;
        }



        // Functie Create Index daily at set hour into SQL Server
        public static void CreateIndexIntoSqlCuptor(string plcName, RaportareDbContext dbContext)
        {

            // _context.Add(plcModel);
            // await _context.SaveChangesAsync();

        }

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
