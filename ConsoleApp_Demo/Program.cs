using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApp_Demo
{
    class Program
    {
        static HubConnection connection;
         static PerformanceCounter cpu = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        static bool readCpu = false;
        static  void Main(string[] args)
        {
            Console.WriteLine("Hello SignalR!");
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44382/sensor")
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.On("StartCpu", () => {
                Cpu();
            });
            bool x = true;
            while (x)
            {
                Console.Clear();
                Console.WriteLine("1. Connect");
                Console.WriteLine("2. Send alert");
                Console.WriteLine("3. Read Cpu usage");
                Console.WriteLine("4. Stop Reading");
                Console.WriteLine("5. Close");
                Console.WriteLine("6. Connect to group");
                string input = Console.ReadLine();
                switch (input)
                {
                     case "1" :  connect(); break;
                     case "2": alert();break;
                     case "3" : Cpu(); break;
                     case "4" : readCpu = false; break;
                     case "5" : x = false; break;
                     case "6" : connectToGroup(); break;
                     default: break;
                }
            }
    }

        private async static void connectToGroup()
        {
            await connection.InvokeAsync("JoinRoom","CPU");
        }

        private async static void Cpu()
        {
            readCpu = true;
            while (readCpu)
            {
                System.Threading.Thread.Sleep(1000);
                await connection.InvokeAsync("Cpu", cpu.NextValue(), DateTime.Now.ToString("hh:mm:ss"));
            }
        }

        private async static void alert()
        {
           

           await  connection.InvokeAsync("Alert",   5, "ALERT " + DateTime.Now.ToString("hh:mm:ss"));
        }

        private async static void connect()
        { 
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 
    }
}
