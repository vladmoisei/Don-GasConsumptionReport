using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Don_GasConsumtionReport.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using S7.Net;
using Microsoft.EntityFrameworkCore;

namespace Don_GasConsumtionReport.Controllers
{
    public class HomeController : Controller
    {
        /*
         Scrie Json in fisier 
         Product product = new Product();
product.ExpiryDate = new DateTime(2008, 12, 28);

JsonSerializer serializer = new JsonSerializer();
serializer.Converters.Add(new JavaScriptDateTimeConverter());
serializer.NullValueHandling = NullValueHandling.Ignore;

using (StreamWriter sw = new StreamWriter(@"c:\json.txt"))
using (JsonWriter writer = new JsonTextWriter(sw))
{
    serializer.Serialize(writer, product);
    // {"ExpiryDate":new Date(1230375600000),"Price":0}
}
             */
        private readonly RaportareDbContext _context;
        private readonly BackgroundService _backGroundService;

        public HomeController(RaportareDbContext context, IHostedService backGroundService)
        {
            _context = context;
            _backGroundService = backGroundService as BackgroundService;
        }
        public IActionResult Index()
        {
            ViewBag.ServiceIsStarted = _backGroundService.IsStartedService.ToString();
            //ViewBag.Val = PlcService.probaIncrementare; proba incrementare
            
            // La prima rulare actualizare in view ultimele elemente adaugate in SQL
            Raport.UpdateLastElements(_context);
            return View();
        }

        #region View Index UpdateParameters - refresh automatically
        // Actiune returnare parametri in Index View pentru actualizare 
        public IActionResult UpdateParameters()
        {
            PassDataToView dataToPass = new PassDataToView
            {
                Clock = Auxiliar.GetClock(),
                IsStartedBackgroundService = _backGroundService.IsStartedService,
                IsCreatedPlcCuptor = PlcService.IsCreatedPlcByName("PlcCuptor"),
                IsConnectedPlcCuptor = PlcService.IsConnectedPlcByName("PlcCuptor"),
                IsCreatedPlcGaddaF2 = PlcService.IsCreatedPlcByName("PlcGaddaF2"),
                IsConnectedPlcGaddaF2 = PlcService.IsConnectedPlcByName("PlcGaddaF2"),
                IsCreatedPlcGaddaF4 = PlcService.IsCreatedPlcByName("PlcGaddaF4"),
                IsConnectedPlcGaddaF4 = PlcService.IsConnectedPlcByName("PlcGaddaF4"),
                TextBoxListaMailCuptor = Raport.ListaMailCuptor,
                TextBoxOraRaportCuptor = Raport.OraRaportCuptor,
                TextBoxListaMailGadda = Raport.ListaMailGadda,
                TextBoxOraRaportGadda = Raport.OraRaportGadda,
                TextBlockIndexCuptor = Raport.IndexCuptor,
                TextBlockIndexGaddaF2 = Raport.IndexGaddaF2,
                TextBlockIndexGaddaF4 = Raport.IndexGaddaF4,
                TextBlockConsumCuptor = Raport.ValoareConsumGazCuptor,
                TextBlockConsumGaddaF2 = Raport.ValoareConsumGazGaddaF2,
                TextBlockConsumGaddaF4 = Raport.ValoareConsumGazGaddaF4,
                TextBlockDataOraRaportFacut = Raport.DataOraRaportFacut


            };

            return new JsonResult(dataToPass);


            //string proba = JsonConvert.SerializeObject(Auxiliar.GetClock(), _backGroundService.IsStartedService.ToString());
            //return new JsonResult(Auxiliar.GetClock());
        }
        #endregion

        public IActionResult CuptorPropulsie()
        {
            return View();
        }

        public IActionResult CuptorGadda()
        {
            return View();
        }

        #region View Index: Comenzi butoane BackgroundService
        // COMENZI BUTOANE INDEX
        //  Stop BackgroundService      
        public async Task<IActionResult> StopBackGroundServiceAsync()
        {
            await _backGroundService.StopAsync(new System.Threading.CancellationToken());

            return RedirectToAction(nameof(Index));
        }

        // Start BackgroundService      
        public async Task<IActionResult> StartBackGroundServiceAsync()
        {
            if (!_backGroundService.IsStartedService)
                await _backGroundService.StartAsync(new System.Threading.CancellationToken());

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region View Index: Comenzi butoane Plc Cuptor
        // COMENZI PLC CUPTOR
        // Create Plc Cuptor
        public IActionResult CreatePlcCuptor()
        {
            if (!PlcService.IsCreatedPlcByIp("172.16.4.104"))
            {
                PlcService.CreatePlc("PlcCuptor", S7.Net.CpuType.S7300, "172.16.4.104", 0, 2);
            }
            return RedirectToAction(nameof(Index));
        }

        // Stergere Plc Cuptor
        public IActionResult DeletePlcCuptor()
        {
            if (PlcService.IsCreatedPlcByIp("172.16.4.104"))
            {
                PlcService.DeletePlcByName("PlcCuptor");
            }
            return RedirectToAction(nameof(Index));
        }

        // Connect Plc Cuptor
        public IActionResult ConnectPlcCuptor()
        {
            if (!PlcService.IsConnectedPlcByName("PlcCuptor"))
            {
                PlcService.ConnectPlcByName("PlcCuptor");
            }
            return RedirectToAction(nameof(Index));
        }

        // Deconnect Plc Cuptor
        public IActionResult DeconnectPlcCuptor()
        {
            if (PlcService.IsConnectedPlcByName("PlcCuptor"))
            {
                PlcService.DeConnectPlcByName("PlcCuptor");
            }
            return RedirectToAction(nameof(Index));
        }

        // Check Ip Manual Plc Cuptor
        public IActionResult CheckIpPlcCuptor()
        {
            return new JsonResult(PlcService.IsAvailableIpAdress("172.16.4.104"));
        }
        #endregion

        #region View Index: Comenzi butoane Plc GaddaF2
        // COMENZI PLC GaddaF2
        // Create Plc GaddaF2
        public IActionResult CreatePlcGaddaF2()
        {
            if (!PlcService.IsCreatedPlcByIp("10.0.0.11"))
            {
                PlcService.CreatePlc("PlcGaddaF2", S7.Net.CpuType.S7300, "10.0.0.11", 0, 2);
            }
            return RedirectToAction(nameof(Index));
        }

        // Stergere Plc GaddaF2
        public IActionResult DeletePlcGaddaF2()
        {
            if (PlcService.IsCreatedPlcByIp("10.0.0.11"))
            {
                PlcService.DeletePlcByName("PlcGaddaF2");
            }
            return RedirectToAction(nameof(Index));
        }

        // Connect Plc GaddaF2
        public IActionResult ConnectPlcGaddaF2()
        {
            if (!PlcService.IsConnectedPlcByName("PlcGaddaF2"))
            {
                PlcService.ConnectPlcByName("PlcGaddaF2");
            }
            return RedirectToAction(nameof(Index));
        }

        // Deconnect Plc GaddaF2
        public IActionResult DeconnectPlcGaddaF2()
        {
            if (PlcService.IsConnectedPlcByName("PlcGaddaF2"))
            {
                PlcService.DeConnectPlcByName("PlcGaddaF2");
            }
            return RedirectToAction(nameof(Index));
        }

        // Check Ip Manual Plc GaddaF2
        public IActionResult CheckIpPlcGaddaF2()
        {
            return new JsonResult(PlcService.IsAvailableIpAdress("10.0.0.11"));
        }
        #endregion

        #region View Index: Comenzi butoane Plc GaddaF4
        // COMENZI PLC GaddaF4
        // Create Plc GaddaF4
        public IActionResult CreatePlcGaddaF4()
        {
            if (!PlcService.IsCreatedPlcByIp("10.0.0.13"))
            {
                PlcService.CreatePlc("PlcGaddaF4", S7.Net.CpuType.S7300, "10.0.0.13", 0, 2);
            }
            return RedirectToAction(nameof(Index));
        }

        // Stergere Plc GaddaF4
        public IActionResult DeletePlcGaddaF4()
        {
            if (PlcService.IsCreatedPlcByIp("10.0.0.13"))
            {
                PlcService.DeletePlcByName("PlcGaddaF4");
            }
            return RedirectToAction(nameof(Index));
        }

        // Connect Plc GaddaF4
        public IActionResult ConnectPlcGaddaF4()
        {
            if (!PlcService.IsConnectedPlcByName("PlcGaddaF4"))
            {
                PlcService.ConnectPlcByName("PlcGaddaF4");
            }
            return RedirectToAction(nameof(Index));
        }

        // Deconnect Plc GaddaF4
        public IActionResult DeconnectPlcGaddaF4()
        {
            if (PlcService.IsConnectedPlcByName("PlcGaddaF4"))
            {
                PlcService.DeConnectPlcByName("PlcGaddaF4");
            }
            return RedirectToAction(nameof(Index));
        }

        // Check Ip Manual Plc GaddaF4
        public IActionResult CheckIpPlcGaddaF4()
        {
            return new JsonResult(PlcService.IsAvailableIpAdress("10.0.0.13"));
        }
        #endregion

        #region View Index: Setare ListaMail si oraRaport; index si consum Cuptor & Gadda
        // Set Lista mail si ora raport Plc Cuptor
        public IActionResult SetListaMailOraRaportPlcCuptor(string listaMail, string oraRaport)
        {
            Raport.ListaMailCuptor = listaMail;
            Raport.OraRaportCuptor = oraRaport;
            //return RedirectToAction(nameof(Index));
            return new JsonResult(new { Lista = Raport.ListaMailCuptor, Ora = Raport.OraRaportCuptor });
        }
        // Show Lista mail si ora raport Plc Cuptor
        public IActionResult ShowListaMailOraRaportPlcCuptor()
        {
            return new JsonResult(new { Lista = Raport.ListaMailCuptor, Ora = Raport.OraRaportCuptor });
        }
        // Set Lista mail si ora raport Plc Gadda
        public IActionResult SetListaMailOraRaportPlcGadda(string listaMail, string oraRaport)
        {
            Raport.ListaMailGadda = listaMail;
            Raport.OraRaportGadda = oraRaport;
            //return RedirectToAction(nameof(Index));
            return new JsonResult(new { Lista = Raport.ListaMailGadda, Ora = Raport.OraRaportGadda });
        }
        // Show Lista mail si ora raport Plc Gadda
        public IActionResult ShowListaMailOraRaportPlcGadda()
        {
            return new JsonResult(new { Lista = Raport.ListaMailGadda, Ora = Raport.OraRaportGadda });
        }
        #endregion

        #region View IndexCuptor: Afisare lista index Cuptor
        public async Task<IActionResult> IndexCuptor()
        {
            List<IndexModel> listaDeAfisat = await _context.IndexModels.ToListAsync();
            return View(listaDeAfisat.Where(model => model.PlcName == "PlcCuptor" && Auxiliar.IsCurrentMonth(Auxiliar.ReturnareDataFromString(model.Data))));
        }
        #endregion

        // Din Template
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
