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
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundService(IServiceScopeFactory scopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _scopeFactory = scopeFactory;
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

            PlcService.probaIncrementare = 10;
            return Task.CompletedTask;
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            Raport.ListaPlc = PlcService.ListaPlc;
            // Scriere Index gaz in Sql DataBase
            using (var scope = _scopeFactory.CreateScope())
            {                
                var dbContext = scope.ServiceProvider.GetRequiredService<RaportareDbContext>();                
                Raport.VerificareOraRaport(Raport.OraRaportCuptor, dbContext);
                
                // La data ora setata se 
                //dbContext.Add(new IndexModel { })
                //_context.Add(plcModel);
                //await _context.SaveChangesAsync();
            }

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
