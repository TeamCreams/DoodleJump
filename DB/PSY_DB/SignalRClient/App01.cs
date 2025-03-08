using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient
{
    public class App01
    {

        public void Run()
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:51472/Chat")
                .Build();


            Task task = new Task(async () =>
            {
                await hubConnection.StartAsync();
                hubConnection.On<string, string>("ReceiveMessage", async (user, message) =>
                {
                    Console.WriteLine($"{user} : {message}");
                });

                hubConnection.On<DateTime>("ReceiveHeartBeat", async (dateTime) =>
                {
                    Console.WriteLine($"{dateTime}");
                });

            });

            task.Start();
            task.Wait();

            while (true) ;
           
        }
    }
}
