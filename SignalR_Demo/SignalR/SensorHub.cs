using Microsoft.AspNetCore.SignalR;
using Quartz;
using SignalR_Demo.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Demo.SignalR
{
    public class SensorHub : Hub
    {
        public static IHubContext<SensorHub> _contextHub;
        private IScheduler _scheduler;

        public SensorHub(IHubContext<SensorHub> context, IScheduler scheduler)
        {
            _contextHub = context;
            _scheduler = scheduler;
            var x = _scheduler;
            try
            {
                StartSensor();

            }
            catch (Exception)
            {
                Console.WriteLine("JobRunnung");
            }
        }
        public async Task Alert(int value,string message)
        {
            
            await _contextHub.Clients.All.SendAsync("Alert", new { x = "ALERT\n"+DateTime.Now.ToString("hh:mm:ss"), y = value });
        }
        public async void StartSensor()
        {

            IJobDetail job = JobBuilder.Create<SenzorTask>().WithIdentity("SenzorTask", "gg").Build();
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(" SenzorTrigger", "gg " )
              .StartNow()
              .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
              .Build();
            try
            {
            await _scheduler.ScheduleJob(job, trigger);

            }
            catch (Exception)
            {

            }
        }

    }
}
