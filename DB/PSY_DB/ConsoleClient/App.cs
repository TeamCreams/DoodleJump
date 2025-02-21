using GameApi.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    internal class App
    {
        public async void Run()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://13.209.47.47/User/GetUserAccount");
            ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
            requestDto.UserName = "test1";
            //requestDto.Password = "test1";
            string json = JsonConvert.SerializeObject(requestDto);
            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }

    }
}
