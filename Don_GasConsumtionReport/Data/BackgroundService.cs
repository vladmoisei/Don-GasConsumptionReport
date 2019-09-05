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
        System.Timers.Timer _timer;
        public bool IsStartedService { get; set; } = false;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 500;
            _timer.Elapsed += DoWork;
            _timer.Start();
            IsStartedService = true;

            PlcService.probaIncrementare = 10;
            return Task.CompletedTask;
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            // To Do
            try
            {
                PlcService.probaIncrementare++;
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
