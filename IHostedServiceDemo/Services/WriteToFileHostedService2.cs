using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServiceDemo.Services
{
    public class WriteToFileHostedService2 : IHostedService, IDisposable
    {
        private readonly IHostingEnvironment environment;
        private readonly string fileName = "RegistroServer2.txt";
        private Timer timer;
        public WriteToFileHostedService2(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService: Process Started " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            timer = new Timer(DoWord, null, TimeSpan.Zero, TimeSpan.FromSeconds(7));
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService: Process Stopped " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoWord(object state)
        {
            WriteToFile("WriteToFileHostedService: Doing some work at " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
        }


        private void WriteToFile(string message)
        {
            var path = $@"{environment.ContentRootPath}\wwwroot\{fileName}";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(message);
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}

