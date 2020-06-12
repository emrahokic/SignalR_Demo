using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ConsoleApp_Demo
{
    class Program
    {
        static HubConnection connection;
        static void Main(string[] args)
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
            bool x = true;
            while (x)
            {
                Console.Clear();
                Console.WriteLine("1. Connect");
                Console.WriteLine("2. Send alert");
                Console.WriteLine("3. Close");
                string input = Console.ReadLine();
                switch (input)
                {
                    
                     case "1" :  connect(); break;
                     case "3" : x = false; break;
                     default: break;
                }
            }
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
