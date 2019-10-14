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
            _backGroundService.Raportare.UpdateLastElements(_context);
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
                IsCreatedPlcCuptor = _backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByName("PlcCuptor"),
                IsConnectedPlcCuptor = _backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcCuptor"),
                IsCreatedPlcGaddaF2 = _backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByName("PlcGaddaF2"),
                IsConnectedPlcGaddaF2 = _backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF2"),
                IsCreatedPlcGaddaF4 = _backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByName("PlcGaddaF4"),
                IsConnectedPlcGaddaF4 = _backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF4"),
                TextBoxListaMailCuptor = _backGroundService.Raportare.ListaMailCuptor,
                TextBoxOraRaportCuptor = _backGroundService.Raportare.OraRaportCuptor,
                TextBoxListaMailGadda = _backGroundService.Raportare.ListaMailGadda,
                TextBoxOraRaportGadda = _backGroundService.Raportare.OraRaportGadda,
                TextBlockIndexCuptor = _backGroundService.Raportare.IndexCuptor,
                TextBlockIndexGaddaF2 = _backGroundService.Raportare.IndexGaddaF2,
                TextBlockIndexGaddaF4 = _backGroundService.Raportare.IndexGaddaF4,
                TextBlockConsumCuptor = _backGroundService.Raportare.ValoareConsumGazCuptor,
                TextBlockConsumGaddaF2 = _backGroundService.Raportare.ValoareConsumGazGaddaF2,
                TextBlockConsumGaddaF4 = _backGroundService.Raportare.ValoareConsumGazGaddaF4,
                TextBlockDataOraRaportFacut = _backGroundService.Raportare.DataOraRaportFacut

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
            if (!_backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByIp("172.16.4.104"))
            {
                _backGroundService.Raportare.PlcServiceObject.CreatePlc("PlcCuptor", S7.Net.CpuType.S7300, "172.16.4.104", 0, 2);
            }
            return RedirectToAction(nameof(Index));
        }

        // Stergere Plc Cuptor
        public IActionResult DeletePlcCuptor()
        {
            if (_backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByIp("172.16.4.104"))
            {
                _backGroundService.Raportare.PlcServiceObject.DeletePlcByName("PlcCuptor");
            }
            return RedirectToAction(nameof(Index));
        }

        // Connect Plc Cuptor
        public IActionResult ConnectPlcCuptor()
        {
            if (!_backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcCuptor"))
            {
                _backGroundService.Raportare.PlcServiceObject.ConnectPlcByName("PlcCuptor");
            }
            return RedirectToAction(nameof(Index));
        }

        // Deconnect Plc Cuptor
        public IActionResult DeconnectPlcCuptor()
        {
            if (_backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcCuptor"))
            {
                _backGroundService.Raportare.PlcServiceObject.DeConnectPlcByName("PlcCuptor");
            }
            return RedirectToAction(nameof(Index));
        }

        // Check Ip Manual Plc Cuptor
        public IActionResult CheckIpPlcCuptor()
        {
            return new JsonResult(_backGroundService.Raportare.PlcServiceObject.IsAvailableIpAdress("172.16.4.104"));
        }
        #endregion

        #region View Index: Comenzi butoane Plc GaddaF2
        // COMENZI PLC GaddaF2
        // Create Plc GaddaF2
        public IActionResult CreatePlcGaddaF2()
        {
            if (!_backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.11"))
            {
                _backGroundService.Raportare.PlcServiceObject.CreatePlc("PlcGaddaF2", S7.Net.CpuType.S7300, "10.0.0.11", 0, 2);
            }
            return RedirectToAction(nameof(Index));
        }

        // Stergere Plc GaddaF2
        public IActionResult DeletePlcGaddaF2()
        {
            if (_backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.11"))
            {
                _backGroundService.Raportare.PlcServiceObject.DeletePlcByName("PlcGaddaF2");
            }
            return RedirectToAction(nameof(Index));
        }

        // Connect Plc GaddaF2
        public IActionResult ConnectPlcGaddaF2()
        {
            if (!_backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF2"))
            {
                _backGroundService.Raportare.PlcServiceObject.ConnectPlcByName("PlcGaddaF2");
            }
            return RedirectToAction(nameof(Index));
        }

        // Deconnect Plc GaddaF2
        public IActionResult DeconnectPlcGaddaF2()
        {
            if (_backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF2"))
            {
                _backGroundService.Raportare.PlcServiceObject.DeConnectPlcByName("PlcGaddaF2");
            }
            return RedirectToAction(nameof(Index));
        }

        // Check Ip Manual Plc GaddaF2
        public IActionResult CheckIpPlcGaddaF2()
        {
            return new JsonResult(_backGroundService.Raportare.PlcServiceObject.IsAvailableIpAdress("10.0.0.11"));
        }
        #endregion

        #region View Index: Comenzi butoane Plc GaddaF4
        // COMENZI PLC GaddaF4
        // Create Plc GaddaF4
        public IActionResult CreatePlcGaddaF4()
        {
            if (!_backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.13"))
            {
                _backGroundService.Raportare.PlcServiceObject.CreatePlc("PlcGaddaF4", S7.Net.CpuType.S7300, "10.0.0.13", 0, 2);
            }
            return RedirectToAction(nameof(Index));
        }

        // Stergere Plc GaddaF4
        public IActionResult DeletePlcGaddaF4()
        {
            if (_backGroundService.Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.13"))
            {
                _backGroundService.Raportare.PlcServiceObject.DeletePlcByName("PlcGaddaF4");
            }
            return RedirectToAction(nameof(Index));
        }

        // Connect Plc GaddaF4
        public IActionResult ConnectPlcGaddaF4()
        {
            if (!_backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF4"))
            {
                _backGroundService.Raportare.PlcServiceObject.ConnectPlcByName("PlcGaddaF4");
            }
            return RedirectToAction(nameof(Index));
        }

        // Deconnect Plc GaddaF4
        public IActionResult DeconnectPlcGaddaF4()
        {
            if (_backGroundService.Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF4"))
            {
                _backGroundService.Raportare.PlcServiceObject.DeConnectPlcByName("PlcGaddaF4");
            }
            return RedirectToAction(nameof(Index));
        }

        // Check Ip Manual Plc GaddaF4
        public IActionResult CheckIpPlcGaddaF4()
        {
            return new JsonResult(_backGroundService.Raportare.PlcServiceObject.IsAvailableIpAdress("10.0.0.13"));
        }
        #endregion

        #region View Index: Setare ListaMail si oraRaport; index si consum Cuptor & Gadda
        // Set Lista mail si ora raport Plc Cuptor
        public IActionResult SetListaMailOraRaportPlcCuptor(string listaMail, string oraRaport)
        {
            _backGroundService.Raportare.ListaMailCuptor = listaMail;
            _backGroundService.Raportare.OraRaportCuptor = oraRaport;
            //return RedirectToAction(nameof(Index));
            return new JsonResult(new { Lista = _backGroundService.Raportare.ListaMailCuptor, Ora = _backGroundService.Raportare.OraRaportCuptor });
        }
        // Show Lista mail si ora raport Plc Cuptor
        public IActionResult ShowListaMailOraRaportPlcCuptor()
        {
            return new JsonResult(new { Lista = _backGroundService.Raportare.ListaMailCuptor, Ora = _backGroundService.Raportare.OraRaportCuptor });
        }
        // Set Lista mail si ora raport Plc Gadda
        public IActionResult SetListaMailOraRaportPlcGadda(string listaMail, string oraRaport)
        {
            _backGroundService.Raportare.ListaMailGadda = listaMail;
            _backGroundService.Raportare.OraRaportGadda = oraRaport;
            //return RedirectToAction(nameof(Index));
            return new JsonResult(new { Lista = _backGroundService.Raportare.ListaMailGadda, Ora = _backGroundService.Raportare.OraRaportGadda });
        }
        // Show Lista mail si ora raport Plc Gadda
        public IActionResult ShowListaMailOraRaportPlcGadda()
        {
            return new JsonResult(new { Lista = _backGroundService.Raportare.ListaMailGadda, Ora = _backGroundService.Raportare.OraRaportGadda });
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
