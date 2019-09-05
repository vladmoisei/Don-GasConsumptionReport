using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Don_GasConsumtionReport.Models;
using Microsoft.Extensions.Hosting;

namespace Don_GasConsumtionReport.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult CuptorPropulsie()
        {
            return View();
        }

        public IActionResult CuptorGadda()
        {
            return View();
        }


        public async Task<IActionResult> StopBackGroundServiceAsync()
        {
            await _backGroundService.StopAsync(new System.Threading.CancellationToken());

            return View("Index");
        }

        public async Task<IActionResult> StartBackGroundServiceAsync()
        {
            await _backGroundService.StartAsync(new System.Threading.CancellationToken());

            return View("Index");
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
