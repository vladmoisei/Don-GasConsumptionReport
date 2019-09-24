using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class Raport
    {
        public static string ListaMailCuptor = "v.moisei@beltrame-group.com";
        public static string ListaMailGadda = "v.moisei@beltrame-group.com";

        public static string OraRaportCuptor = "07:00:00";
        public static string OraRaportGadda = "07:00:00";

        public static string DataOraRaportFacut = "0";
        // Index GAz
        public static uint IndexCuptor { get; set; } = 0;
        public static uint IndexGaddaF2 { get; set; } = 0;
        public static uint IndexGaddaF4 { get; set; } = 0;

        // Valori consum gaz
        public static int ValoareConsumGazCuptor { get; set; } = 0;
        public static int ValoareConsumGazGaddaF2 { get; set; } = 0;
        public static int ValoareConsumGazGaddaF4 { get; set; } = 0;
        // TO DO

        /*
         * Functii inregistrare date (index, consum) in SQL Server
         */


        // Functie Creare Index Object pentru salvare in SQL in functie de nume plc
        public static IndexModel GetIndexModelObject(string numePlc, uint valoareIndex)
        {
            IndexModel indexModel = new IndexModel { Data = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), PlcName = numePlc, IndexValue =  (int)valoareIndex};
            return indexModel;
        }

        // Functie Set Consum gaz zilnic
        public static IndexModel AddToIndexModelGasValue(IndexModel indexModel)
        {
            // Salvare valoare index citita in varabile statice raport
            switch (indexModel.PlcName)
            {
                case "PlcCuptor":
                    if (IndexCuptor != 0) ValoareConsumGazCuptor = (int)indexModel.IndexValue - (int)IndexCuptor;
                    indexModel.GazValue = ValoareConsumGazCuptor;
                    break;
                case "PlcGaddaF2":
                    if (IndexGaddaF2 != 0)  ValoareConsumGazGaddaF2 = (int)indexModel.IndexValue - (int)IndexGaddaF2;
                    indexModel.GazValue = ValoareConsumGazGaddaF2;
                    break;
                case "PlcGaddaF4":
                    if (IndexGaddaF4 != 0) ValoareConsumGazGaddaF4 = (int)indexModel.IndexValue - (int)IndexGaddaF4;
                    indexModel.GazValue = ValoareConsumGazGaddaF4;
                    break;
                default:
                    break;
            }

            return indexModel;
        }

        // Functie salvare in database SQL index gaz model
        public static void AddToSqlIndex(RaportareDbContext context, IndexModel indexModel)
        {
            context.Add(AddToIndexModelGasValue(indexModel));
            context.SaveChanges();
            DataOraRaportFacut = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        // Functie get last element added from Sql
        public static IndexModel GetLastElementFromSql(string plcName, RaportareDbContext context)
        {
            // Salvare valoare index citita in varabile statice raport
            switch (plcName)
            {
                case "PlcCuptor":                    
                    if (IndexCuptor == 0)
                    {
                        return context.IndexModels.ToList().Where(model => model.PlcName == "PlcCuptor").LastOrDefault();               
                    }                    
                    break;
                case "PlcGaddaF2":
                    if (IndexGaddaF2 == 0)
                    {
                        return context.IndexModels.ToList().Where(model => model.PlcName == "PlcGaddaF2").LastOrDefault();
                    }
                    break;
                case "PlcGaddaF4":
                    if (IndexGaddaF4 == 0)
                    {
                        return context.IndexModels.ToList().Where(model => model.PlcName == "PlcGaddaF4").LastOrDefault();
                    }
                    break;
                default:
                    break;
            }
            return new IndexModel { IndexValue = 0, GazValue=0, PlcName ="" };
        }

        // Functie actualizare a ultimului element la prima rulare in fiecare lista 
        public static void UpdateLastElements(RaportareDbContext context)
        {
           if (IndexCuptor == 0)
            {
                IndexModel lastElementCuptor = GetLastElementFromSql("PlcCuptor", context);
                IndexCuptor = (uint)lastElementCuptor.IndexValue;
                ValoareConsumGazCuptor = lastElementCuptor.GazValue;
                DataOraRaportFacut = lastElementCuptor.Data;
            }
           if (IndexGaddaF2 == 0)
            {
                IndexModel lastElementGaddaF2 = GetLastElementFromSql("PlcGaddaF2", context);
                IndexGaddaF2 = (uint) lastElementGaddaF2.IndexValue;
                ValoareConsumGazGaddaF2 = lastElementGaddaF2.GazValue;
                DataOraRaportFacut = lastElementGaddaF2.Data;
            }
            if (IndexGaddaF4 == 0)
            {
                IndexModel lastElementGaddaF4 = GetLastElementFromSql("PlcGaddaF4", context);
                IndexGaddaF4 = (uint)lastElementGaddaF4.IndexValue;
                ValoareConsumGazGaddaF4 = lastElementGaddaF4.GazValue;
                DataOraRaportFacut = lastElementGaddaF4.Data;
            }
        }

        // Functie verificare ora raport
        // Setare Index si consum gaz PLC Cuptor, GaddaF2, GaddaF4 pt Plc-urile conectate
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
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexCuptor = plc.ValoareIndexGaz;
                            break;
                        case "PlcGaddaF2":                            
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexGaddaF2 = plc.ValoareIndexGaz; //Dint
                            break;
                        case "PlcGaddaF4":                            
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexGaddaF4 = plc.ValoareIndexGaz; //Dint
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
