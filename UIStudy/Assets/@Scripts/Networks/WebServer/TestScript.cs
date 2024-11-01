using UnityEngine;
using GameApi.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using WebApi.Models.Dto;
public class TestScript : MonoBehaviour
{
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.V))
        {
            Run();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
            requestDto.UserName = "test1";
            requestDto.Password = "test1";
            Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>
            {
                CommonResult<ResDtoGetUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);

                if(rv.IsSuccess == false)
                {
                    
                }

                Debug.Log(rv.Data.RegisterDate);
            });
        }
    }

    public async void Run()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://13.209.47.47/User/GetUserAccount");
        ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
        requestDto.UserName = "test1";
        requestDto.Password = "test1";
        string json = JsonConvert.SerializeObject(requestDto);
        var content = new StringContent(json, null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Debug.Log(await response.Content.ReadAsStringAsync());

    }
}
