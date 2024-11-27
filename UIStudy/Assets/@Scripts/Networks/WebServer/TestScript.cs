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
            //Managers.Event.TriggerEvent(Define.EEventType.SignIn); //호출됨
            //InsertUser();

/*
            ReqDtoGetUserAccountId requestDto = new ReqDtoGetUserAccountId();
            requestDto.UserName = "test1";
            Managers.Web.SendGetRequest(WebRoute.GetUserAccountId(requestDto), (response) =>
            {
                Debug.Log("Response: " + response); 
                CommonResult<ResDtoGetUserAccountId> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccountId>>(response);
                Debug.Log("IsSuccess: " + rv.IsSuccess); 
                if(rv.IsSuccess == true)
                {
                    Debug.Log("success");
                }
                else
                {
                    Debug.Log("already ID!!");
                }
            });
*/
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
            requestDto.UserName = "test";
            //requestDto.Password = "test1";
            Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>
            {
                CommonResult<ResDtoGetUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);

                if(rv.IsSuccess == false)
                {
                    
                }

                Debug.Log(rv.Data.UserName);
            });
        }
    }

    public async void Run()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-single-api.snapism.net:8082/User/GetUserAccount");
        ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
        requestDto.UserName = "test";
        //requestDto.Password = "test1";
        string json = JsonConvert.SerializeObject(requestDto);
        var content = new StringContent(json, null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Debug.Log(await response.Content.ReadAsStringAsync());

    }

    private async void InsertUser()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-single-api.snapism.net:8082/User/InsertUser");
        ReqDtoInsertUserAccount requestDto = new ReqDtoInsertUserAccount();
        requestDto.UserName = "tjdbssy137";
        requestDto.Password = "akfxlwm";
        string json = JsonConvert.SerializeObject(requestDto);
        var content = new StringContent(json, null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
