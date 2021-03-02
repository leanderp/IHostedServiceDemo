using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServiceDemo.Services
{
    public class WriteToFileHostedService : IHostedService
    {
        private readonly IHostingEnvironment environment;
        private readonly string fileName = "File1.txt";

        public WriteToFileHostedService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        
        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService: Process Started");
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService: Process Stopped");
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {
            var path = $@"{environment.ContentRootPath}\wwwroot\{fileName}";
            using (StreamWriter writer = new StreamWriter(path,append: true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
