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
        // Lista Plc Objects
        List<PlcObjectModel> ListaPlc;
        // Variable Scope factory, furnizor servicii
        private readonly IServiceScopeFactory _scopeFactory;
        // Variable DbContext
        private RaportareDbContext dbContext;
        // Variable Scope supply
        private IServiceScope scope;

        public BackgroundService(IServiceScopeFactory scopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _scopeFactory = scopeFactory;
            // Initializare lista plc object
            ListaPlc = new List<PlcObjectModel>();
        }

        System.Timers.Timer _timer;
        public bool IsStartedService { get; set; } = false;

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
            // Transfer lista plc din backgroundService in PlcService
            PlcService.ListaPlc = ListaPlc;

            PlcService.probaIncrementare = 10;
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
                Raport.VerificareOraRaport(Raport.OraRaportCuptor, dbContext);
                
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

            PlcService.probaIncrementare = 100;
            _timer.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

    }
}
