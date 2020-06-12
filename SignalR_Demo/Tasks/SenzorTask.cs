using Microsoft.AspNetCore.SignalR;
using Quartz;
using SignalR_Demo.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Demo.Tasks
{
    public class SenzorTask : IJob
    {
    
        public async Task Execute(IJobExecutionContext context)
        {

            var rand = new Random();
            Console.WriteLine(DateTime.Now.ToString());

            await SensorHub._contextHub.Clients.All.SendAsync("SenzorChange", new { x = DateTime.Now.ToString("hh:mm:ss"), y = rand.NextDouble() });

        }
    }
}
