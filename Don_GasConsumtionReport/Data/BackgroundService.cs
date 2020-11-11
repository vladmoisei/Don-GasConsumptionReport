using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Don_GasConsumtionReport
{
    public class BackgroundService : IHostedService, IDisposable
    {
        // Raport 
        public Raport Raportare;
        // Variable Scope factory, furnizor servicii
        private readonly IServiceScopeFactory _scopeFactory;
        // Variable DbContext
        private RaportareDbContext dbContext;
        // Variable Scope supply
        private IServiceScope scope;
        // Timer
        System.Timers.Timer _timer;
        // Stare serviciu
        public bool IsStartedService { get; set; } = false;
        // Lista mail
        public string ListaTrimitereMail { get; set; } = "v.moisei@beltrame-group.com, a.cernat@beltrame-group.com, "+
                "m.mitran@beltrame-group.com, b.mitran@beltrame-group.com, i.sutu@donalam.ro, i.micu@donalam.ro, m.apopei@donalam.ro";
        // Ora Raportare
        public string OraTrimitereMail { get; set; } = "06:59:00";

        // Constructor
        public BackgroundService(IServiceScopeFactory scopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _scopeFactory = scopeFactory;
            // Initializare lista plc object
            if (Raportare== null) Raportare = new Raport();
            // Actualizare trimitere mail
            Raportare.ListaMailCuptor = ListaTrimitereMail;
            Raportare.OraRaportCuptor = OraTrimitereMail;

            // Cream si conectam PLC-uri 
            // Plc Cuptor
            if (!Raportare.PlcServiceObject.IsCreatedPlcByIp("172.16.4.104"))
            {
                Raportare.PlcServiceObject.CreatePlc("PlcCuptor", S7.Net.CpuType.S7300, "172.16.4.104", 0, 2);
            }

            if (!Raportare.PlcServiceObject.IsConnectedPlcByName("PlcCuptor"))
            {
                Raportare.PlcServiceObject.ConnectPlcByName("PlcCuptor");
            }

            // Plc GaddaF2
            if (!Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.11"))
            {
                Raportare.PlcServiceObject.CreatePlc("PlcGaddaF2", S7.Net.CpuType.S7300, "10.0.0.11", 0, 2);
            }
            if (!Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF2"))
            {
                Raportare.PlcServiceObject.ConnectPlcByName("PlcGaddaF2");
            }

            // Plc GaddaF4
            if (!Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.13"))
            {
                Raportare.PlcServiceObject.CreatePlc("PlcGaddaF4", S7.Net.CpuType.S7300, "10.0.0.13", 0, 2);
            }
            if (!Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF4"))
            {
                Raportare.PlcServiceObject.ConnectPlcByName("PlcGaddaF4");
            }

            // Plc Elti
            if (!Raportare.PlcServiceObject.IsCreatedPlcByIp("172.16.4.70"))
            {
                Raportare.PlcServiceObject.CreatePlc("PlcElti", S7.Net.CpuType.S7300, "172.16.4.70", 0, 2);
            }
            if (!Raportare.PlcServiceObject.IsConnectedPlcByName("PlcElti"))
            {
                Raportare.PlcServiceObject.ConnectPlcByName("PlcElti");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 700;
            _timer.Elapsed += DoWork;
            _timer.Start();
            IsStartedService = true;

            // Start Scope service pentru a returna dbContext
            scope = _scopeFactory.CreateScope();
            // Creare dBContext din Scope
            dbContext = scope.ServiceProvider.GetRequiredService<RaportareDbContext>();                       

            //PlcService.probaIncrementare = 10;
            return Task.CompletedTask;
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            //Raport.ListaPlc = PlcService.ListaPlc;
            // Scriere Index gaz in Sql DataBase
            //using (var scope = _scopeFactory.CreateScope())
            //{                
                //var dbContext = scope.ServiceProvider.GetRequiredService<RaportareDbContext>();                
                Raportare.VerificareOraRaport(Raportare.OraRaportCuptor, dbContext);
                
                // La data ora setata se 
                //dbContext.Add(new IndexModel { })
                //_context.Add(plcModel);
                //await _context.SaveChangesAsync();
            //}

            try
            {
                //PlcService.probaIncrementare++; incrementare de proba
            }
            catch (NullReferenceException exNull)
            {

                throw;
            }
            _timer.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            IsStartedService = false;
            //PlcService.probaIncrementare = 100;
            _timer.Dispose();

            // Deconectare si stergere Plc

            // Plc Cuptor
            if (Raportare.PlcServiceObject.IsConnectedPlcByName("PlcCuptor"))
            {
                Raportare.PlcServiceObject.DeConnectPlcByName("PlcCuptor");
            }
            if (Raportare.PlcServiceObject.IsCreatedPlcByIp("172.16.4.104"))
            {
                Raportare.PlcServiceObject.DeletePlcByName("PlcCuptor");
            }

            // Plc GaddaF2
            if (Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF2"))
            {
                Raportare.PlcServiceObject.DeConnectPlcByName("PlcGaddaF2");
            }
            if (Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.11"))
            {
                Raportare.PlcServiceObject.DeletePlcByName("PlcGaddaF2");
            }

            // Plc GadaF4
            if (Raportare.PlcServiceObject.IsConnectedPlcByName("PlcGaddaF4"))
            {
                Raportare.PlcServiceObject.DeConnectPlcByName("PlcGaddaF4");
            }
            if (Raportare.PlcServiceObject.IsCreatedPlcByIp("10.0.0.13"))
            {
                Raportare.PlcServiceObject.DeletePlcByName("PlcGaddaF4");
            }

            // Plc Elti
            if (Raportare.PlcServiceObject.IsConnectedPlcByName("PlcElti"))
            {
                Raportare.PlcServiceObject.DeConnectPlcByName("PlcElti");
            }
            if (Raportare.PlcServiceObject.IsCreatedPlcByIp("172.16.4.70"))
            {
                Raportare.PlcServiceObject.DeletePlcByName("PlcElti");
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

    }
}
