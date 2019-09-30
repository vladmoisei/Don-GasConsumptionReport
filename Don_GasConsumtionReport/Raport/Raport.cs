using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
            IndexModel indexModel = new IndexModel { Data = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), PlcName = numePlc, IndexValue = (int)valoareIndex };
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
                    if (IndexGaddaF2 != 0) ValoareConsumGazGaddaF2 = (int)indexModel.IndexValue - (int)IndexGaddaF2;
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

            return new IndexModel { IndexValue = 0, GazValue = 0, PlcName = "" };
        }

        // Functie actualizare a ultimului element la prima rulare in fiecare lista 
        public static void UpdateLastElements(RaportareDbContext context)
        {
            try
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
                    IndexGaddaF2 = (uint)lastElementGaddaF2.IndexValue;
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
            catch (NullReferenceException ex)
            {
            }
        }

        // Functie verificare daca este data 01 a lunii
        public static bool IsFirstDayOfMonth()
        {
            if (DateTime.Now.Day == 1) return true;
            return false;
        }

        // Functie salvare fisiere pentru luna precedenta pe data de 1 a lunii
        public static string SaveExcelFilesForLastMonth(string numePlc, RaportareDbContext context)
        {
            if (IsFirstDayOfMonth())
            {
                string dataFrom = DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy");
                string dataTo = DateTime.Now.ToString("dd.MM.yyyy");
                return SaveExcelFileToDisk(dataFrom, dataTo, numePlc, context);
            }
            return "";
        }

        // Functie verificare ora raport
        // Setare Index si consum gaz PLC Cuptor, GaddaF2, GaddaF4 pt Plc-urile conectate
        public static bool VerificareOraRaport(string ora, RaportareDbContext context)
        {
            // La 10 minute verificam conexiune PLc-uri, si daca nu e facuta, incercam sa o refacem
            if (DateTime.Now.ToString("HH:mm:ss").Substring(4, 4) == "5:00")
            {
                // Verificare conexiune Plc-uri, tinem in viata conexiunea cu plc-urile
                foreach (PlcObjectModel plc in PlcService.ListaPlc)
                {
                    if (!plc._client.IsConnected)
                        if (plc.IsAvailableIpAdress())
                            plc.ConnectPlc();
                }
            }
            // Se verifica daca este ora raport si se inregistreaza date in SQL, se trimite mail
            if (DateTime.Now.ToString("HH:mm:ss") == ora)
            {
                string filePathCuptor = "";
                string filePathGaddaF2 = "";
                string filePathGaddaF4 = "";
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
                            filePathCuptor = SaveExcelFilesForLastMonth(plc.PlcName, context);
                            break;
                        case "PlcGaddaF2":
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexGaddaF2 = plc.ValoareIndexGaz; //Dint
                                                                // Creare Fisier excel cu consumul pe luna precedenta pe 1 a lunii
                            filePathGaddaF2 = SaveExcelFilesForLastMonth(plc.PlcName, context);
                            break;
                        case "PlcGaddaF4":
                            AddToSqlIndex(context, GetIndexModelObject(plc.PlcName, plc.ValoareIndexGaz));
                            IndexGaddaF4 = plc.ValoareIndexGaz; //Dint
                                                                // Creare Fisier excel cu consumul pe luna precedenta pe 1 a lunii
                            filePathGaddaF4 = SaveExcelFilesForLastMonth(plc.PlcName, context);
                            break;
                        default:
                            break;
                    }
                }

                // Trimitere mail
                string subiectTextMailDaily = String.Format("Consum gaz Cuptor, Gadda pe data: {0}",
                    DateTime.Now.ToString("dd MMMM, yyyy"));
                string bodyTextMailDaily = ReturnBodyMailTextDaily(GetJsonForMail());
                SendEmaildaily(ListaMailCuptor, subiectTextMailDaily, bodyTextMailDaily); // functie trimitere mail zilnic

                // Functie trimitere mail lunar cu consumul inregistrat pe toata luna pe dat ade 1 a lunii
                if (IsFirstDayOfMonth())
                {
                    string subiectTextMailMonthly = String.Format("Consum gaz Cuptor, Gadda F2 si F4 pe luna: {0}",
                        DateTime.Now.AddMonths(-1).ToString("MMMM, yyyy"));
                    string bodyTextMailMonthly = string.Format("Buna dimineata. <br><br>Atasat gasiti consumul de gaz " +
                        "inregistrat de contor gaz cuptor cu propuslie, Gadda F2 si F4 pe luna {0}. <br><br>O zi buna.",
                    DateTime.Now.AddMonths(-1).ToString("MMMM, yyyy"));
                    // Functie trimitere mail lunar
                    SendEmailmonthly(ListaMailGadda, subiectTextMailMonthly, bodyTextMailMonthly, filePathCuptor,
                        filePathGaddaF2, filePathGaddaF4);
                }
                System.Threading.Thread.Sleep(1000);
                return true;
            }
            return false;
        }


        /*
         * Functii trimitere mail zilnic si lunar
         */

        // Functie trimitere mail
        public static void TrimitereRaportMail(string adreseMailDeTrimis, string filePathDeTrimis, string subiect)
        {
            try
            {
                // "don.rap.ajustaj@gmail.com", "v.moisei@beltrame-group.com, vladmoisei@yahoo.com"
                // Mail(emailFrom , emailTo)
                MailMessage mail = new MailMessage("don.rap.ajustaj@gmail.com", adreseMailDeTrimis);

                //mail.From = new MailAddress("don.rap.ajustaj@gmail.com");
                mail.Subject = "Consum gaz cuptor cu propulsie";
                string Body = string.Format("Buna dimineata. <br>Atasat gasiti consumul de gaz inregistrat de contor gaz " +
                    "cuptor cu propuslie pe luna {0}. <br>O zi buna.", DateTime.Now.AddMonths(-1).ToString("MMMM, yyyy"));
                mail.Body = Body;
                mail.IsBodyHtml = true;
                using (Attachment attachment = new Attachment(filePathDeTrimis))
                {
                    mail.Attachments.Add(attachment);

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("don.rap.ajustaj@gmail.com", "Beltrame.1");
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    mail = null;
                    smtp = null;
                }

                // Console.WriteLine("Mail Sent succesfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                // Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        // Functie Send Email daily with Consumption of Gas
        public static void SendEmaildaily(string adreseMailDeTrimis, string subiectText, string bodyText)
        {
            try
            {
                // "don.rap.ajustaj@gmail.com", "v.moisei@beltrame-group.com, vladmoisei@yahoo.com"
                // Mail(emailFrom , emailTo)
                MailMessage mail = new MailMessage("don.rap.ajustaj@gmail.com", adreseMailDeTrimis);

                //mail.From = new MailAddress("don.rap.ajustaj@gmail.com");
                mail.Subject = subiectText;
                string Body = string.Format("Buna dimineata. <br>Atasat gasiti consumul de gaz inregistrat de contor gaz " +
                    "cuptor cu propuslie pe luna {0}. <br>O zi buna.", DateTime.Now.AddMonths(-1).ToString("MMMM, yyyy"));
                mail.Body = bodyText;
                mail.IsBodyHtml = true;
                //using (Attachment attachmentCuptor = new Attachment(""), attachmentGaddaF2 = new Attachment(""), attachmentGaddaF4 = new Attachment(""))
                //{
                //    mail.Attachments.Add(attachment);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential("don.rap.ajustaj@gmail.com", "Beltrame.1");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);

                mail = null;
                smtp = null;
                //}

                // Console.WriteLine("Mail Sent succesfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                // Console.WriteLine(ex.ToString());
                //throw ex;
            }
        }

        // Functie Send Email monthly with Consumption for all month
        public static void SendEmailmonthly(string adreseMailDeTrimis, string subiectText, string bodyText,
            string filePathDeTrimisCuptor, string filePathDeTrimisGaddaF2, string filePathDeTrimisGaddaF4)
        {
            try
            {
                // "don.rap.ajustaj@gmail.com", "v.moisei@beltrame-group.com, vladmoisei@yahoo.com"
                // Mail(emailFrom , emailTo)
                MailMessage mail = new MailMessage("don.rap.ajustaj@gmail.com", adreseMailDeTrimis);

                //mail.From = new MailAddress("don.rap.ajustaj@gmail.com");
                mail.Subject = subiectText;
                string Body = string.Format("Buna dimineata. <br>Atasat gasiti consumul de gaz inregistrat de contor gaz " +
                    "cuptor cu propuslie pe luna {0}. <br>O zi buna.", DateTime.Now.AddMonths(-1).ToString("MMMM, yyyy"));
                mail.Body = bodyText;
                mail.IsBodyHtml = true;
                using (Attachment attachmentCuptor = new Attachment(filePathDeTrimisCuptor),
                    attachmentGaddaF2 = new Attachment(filePathDeTrimisGaddaF2),
                    attachmentGaddaF4 = new Attachment(filePathDeTrimisGaddaF4))
                {
                    mail.Attachments.Add(attachmentCuptor);
                    mail.Attachments.Add(attachmentGaddaF2);
                    mail.Attachments.Add(attachmentGaddaF4);

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("don.rap.ajustaj@gmail.com", "Beltrame.1");
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    mail = null;
                    smtp = null;
                }

                // Console.WriteLine("Mail Sent succesfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                // Console.WriteLine(ex.ToString());
                //throw ex;
            }
        }

        // Functie return Json cu text varabile index si consum pentru cuptor si Gadda
        public static string GetJsonForMail()
        {
            //    // Index GAz
            //public static uint IndexCuptor { get; set; } = 0;
            //public static uint IndexGaddaF2 { get; set; } = 0;
            //public static uint IndexGaddaF4 { get; set; } = 0;

            //// Valori consum gaz
            //public static int ValoareConsumGazCuptor { get; set; } = 0;
            //public static int ValoareConsumGazGaddaF2 { get; set; } = 0;
            //public static int ValoareConsumGazGaddaF4 { get; set; } = 0;
            string json = JsonConvert.SerializeObject(new
            {
                indexCuptor = IndexCuptor,
                indexGaddaF2 = IndexGaddaF2,
                indexGaddaF4 = IndexGaddaF4,
                valoareConsumGazCuptor = ValoareConsumGazCuptor,
                valoareConsumGazGaddaF2 = ValoareConsumGazGaddaF2,
                valoareConsumGazGaddaF4 = ValoareConsumGazGaddaF4
            });
            return json;
        }

        // Functie setare text body mail daily cu consum gaz
        public static string ReturnBodyMailTextDaily(string json)
        {
            var definition = new
            {
                indexCuptor = 0,
                indexGaddaF2 = 0,
                indexGaddaF4 = 0,
                valoareConsumGazCuptor = 0,
                valoareConsumGazGaddaF2 = 0,
                valoareConsumGazGaddaF4 = 0
            };

            var jsonResult = JsonConvert.DeserializeAnonymousType(json, definition);

            string Body = string.Format("Buna dimineata. <br>Atasat gasiti indexul cat si consumul de gaz pentru: " +
                "cuptor cu propuslie, Gadda cuptor F2, Gadda cuptor F4:<br>" +
                "<br>Index Cuptor: {0}" +
                "<br>Index GaddaF2: {1}" +
                "<br>Index GaddaF4: {2}<br>" +
                "<br>Consum Cuptor: <strong>{3}</strong>" +
                "<br>Consum GaddaF2: <strong>{4}</strong>" +
                "<br>Consum GaddaF4: <strong>{5}</strong><br>" +
                "<br>O zi buna.",
                jsonResult.indexCuptor,
                jsonResult.indexGaddaF2,
                jsonResult.indexGaddaF4,
                jsonResult.valoareConsumGazCuptor,
                jsonResult.valoareConsumGazGaddaF2,
                jsonResult.valoareConsumGazGaddaF4);
            return Body;
        }

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
        // returneaza FilePath
        public static string SaveExcelFileToDisk(string dataFrom, string dataTo, string numePlc, RaportareDbContext _context)
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
                return filePath;
            }

        }

        // Functie daca lista PLC este goala, cream PLC-uri si le conectam
        public static void RemakePlcConnectionWhenAvailable(string numePlc)
        {

        }

    }
}
