using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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

        // Functie verificare daca este data 01 a lunii
        public static bool IsFirstDayOfMonth()
        {
            if (DateTime.Now.Day == 1) return true;
            return false;
        }

        // Functie salvare fisiere pentru luna precedenta pe data de 1 a lunii
        public static void SaveExcelFilesForLastMonth(string numePlc, RaportareDbContext context)
        {
            if (IsFirstDayOfMonth())
            {
                string dataFrom = DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy");
                string dataTo = DateTime.Now.ToString("dd.MM.yyyy");
                SaveExcelFileToDisk(dataFrom, dataTo, numePlc, context);
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
                            // Creare Fisier excel cu consumul pe luna precedenta pe 1 a lunii
                            SaveExcelFilesForLastMonth(plc.PlcName, context);
                            break;
                        case "PlcGaddaF2":                            
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexGaddaF2 = plc.ValoareIndexGaz; //Dint
                            // Creare Fisier excel cu consumul pe luna precedenta pe 1 a lunii
                            SaveExcelFilesForLastMonth(plc.PlcName, context);
                            break;
                        case "PlcGaddaF4":                            
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexGaddaF4 = plc.ValoareIndexGaz; //Dint
                            // Creare Fisier excel cu consumul pe luna precedenta pe 1 a lunii
                            SaveExcelFilesForLastMonth(plc.PlcName, context);
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



        /*
         * Functii trimitere mail zilnic si lunar
         */

        // Functie Send Email daily with Consumption
        public static void SendEmaildaily(string listaMail) { }

        // Functie Send Email monthly with Consumption for all month
        public static void SendEmailmonthly(string listaMail) { }

        /*
         * Functii creare folder si salvare fisiere exvcel cu consumul lunar
         */
        // Creare folder stocare fisiere consum gaz
        public static string CreareFolderRaportare(string numePlc)
        {
            // Daca pui direct folderul de exp. Consum ... se salveaza in folderul radacina al proiectului
            string path = string.Format(@"c:\Consum gaz/{0}/{1}", DateTime.Now.ToString("yyyy"), numePlc);
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    // Console.WriteLine("That path exists already.");
                    return path;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                // Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
                //di.Delete();
                //Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception ex)
            {
                // Console.WriteLine("The process failed: {0}", e.ToString());
                //MessageBox.Show(ex.Message);
                //throw ex;
            }

            return path;
        }

        // Functie creare fisier excel cu consumul lunar
        // Functie exportare data to excel file and save to disk
        public static void SaveExcelFileToDisk(string dataFrom, string dataTo, string numePlc, RaportareDbContext _context)
        {
            //return Content(dataFrom + "<==>" + dataTo);
            List<IndexModel> listaSql = _context.IndexModels.ToList();
            IEnumerable<IndexModel> listaExcel = listaSql.Where(model => model.PlcName == numePlc);

            // Extrage datele cuprinse intre limitele date de operator
            IEnumerable<IndexModel> listaDeAfisat = listaExcel.Where(model => Auxiliar.IsDateBetween(model.Data, dataFrom, dataTo));

            //var stream = new MemoryStream();

            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(numePlc);
                ws.Cells["A1:Z1"].Style.Font.Bold = true;

                ws.Cells["A1"].Value = "Id";
                ws.Cells["B1"].Value = "Data";
                ws.Cells["C1"].Value = "Nume Plc";
                ws.Cells["D1"].Value = "Index gaz";
                ws.Cells["E1"].Value = "Consum gaz";

                int rowStart = 2;
                foreach (var elem in listaDeAfisat)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Value = elem.Id;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = elem.Data;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = elem.PlcName;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = elem.IndexValue;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = elem.GazValue;
                    rowStart++;
                }

                ws.Cells["A:AZ"].AutoFitColumns();

                //Write the file to the disk
                string excelName = "RaportGaz_" + numePlc + "_" + DateTime.Now.AddMonths(-1).ToString("MMMM") + ".xlsx";
                string filePath = string.Format("{0}/{1}", Raport.CreareFolderRaportare(numePlc), excelName);
                FileInfo fi = new FileInfo(filePath);
                pck.SaveAs(fi);
                //pck.Save();


            }
            //stream.Position = 0;
            

            

            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }
    }
}
