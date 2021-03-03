﻿using IHostedServiceDemo.Data;
using IHostedServiceDemo.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServiceDemo.Services
{
    public class ConsumeScopedService : IHostedService
    {
        public ConsumeScopedService(IServiceProvider services)
        {
            Services = services;
        }

        private Timer _timer;

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWord, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            return Task.CompletedTask;
        }

        private void DoWord(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var message = "ConsumeScopedService. Received message at " + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                var log = new HostedServiceLogs() { Message = message };
                context.HostedServiceLogs.Add(log);
                context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}