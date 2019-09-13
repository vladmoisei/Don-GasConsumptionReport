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
            ViewBag.Val = PlcService.probaIncrementare;

            return View();
        }

        // Actiune returnare parametri in Index View pentru actualizare 
        public IActionResult UpdateParameters()
        {
            PassDataToView dataToPass = new PassDataToView
            {
                Clock = Auxiliar.GetClock(),
                IsStartedBackgroundService = _backGroundService.IsStartedService,
                IsCreatedPlcCuptor = PlcService.IsCreatedPlcByName("PlcCuptor"),
                IsConnectedPlcCuptor = PlcService.IsConnectedPlcByName("PlcCuptor")
            };

            return new JsonResult(dataToPass);


            //string proba = JsonConvert.SerializeObject(Auxiliar.GetClock(), _backGroundService.IsStartedService.ToString());
            //return new JsonResult(Auxiliar.GetClock());
        }

        public IActionResult CuptorPropulsie()
        {
            return View();
        }

        public IActionResult CuptorGadda()
        {
            return View();
        }


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
