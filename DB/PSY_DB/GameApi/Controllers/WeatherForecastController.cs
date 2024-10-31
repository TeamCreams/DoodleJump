using GameApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PSY_DB;
using PSY_DB.Tables;
using System.Text.Json.Serialization;
using WebApi.Models.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PsyDbContext _context;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, PsyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetUser")]
        public async Task<string> GetUser()
        {
            var a = await (from userAccount in _context.TblUserAccounts
                    select new
                    {
                        UserName = userAccount.UserName,
                    }).ToListAsync();

            return JsonConvert.SerializeObject(a);
        }

        //Get - FromQuery
        //Post - FromBody
        [HttpPost("AddUser")]
        public async Task<CommonResult<ResDtoAddUser>> AddUser([FromBody] ReqDtoAddUser requestDto)
        {
            CommonResult<ResDtoAddUser> rv = new();

            try{ 
                var userAccounts = await (from user in _context.TblUserAccounts
                                          where user.UserName == requestDto.UserName 
                                          select new
                                          {
                                              UserName = user.UserName
                                          }).ToListAsync();

                if (userAccounts.Count < 1)
                {
                    TblUserAccount userAccount = new();
                    userAccount.RegisterDate = DateTime.Now;
                    userAccount.UpdateDate = DateTime.Now;
                    userAccount.UserName = requestDto.UserName;
                    userAccount.Password = requestDto.Password;
                    _context.TblUserAccounts.Add(userAccount);
                }
                
                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.NameAlreadyExists, $"Name Already Exists");
                }
                else
                {
                    rv.IsSuccess = true;
                    rv.StatusCode = EStatusCode.OK;
                    rv.Data = null;
                }
            }
            catch (CommonException ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.Data = null;
            }
            catch (Exception ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
                rv.Data = null;
            }
            return rv;
        }

        [HttpPost("SearchUser")]
        public async Task<CommonResult<ResDtoSearchUser>> SearchUser([FromBody] ReqDtoSearchUser requestDto)
        {
            CommonResult<ResDtoSearchUser> rv = new();

            try
            {
                rv.Data = new();

                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.UserName == requestDto.UserName && user.Password == requestDto.Password)
                            select new ResDtoSearchUser                            
                            {
                                UserName = user.UserName,
                                RegisterDate = user.RegisterDate,
                                UpdateDate = user.UpdateDate,
                            }).ToListAsync();


                if (select.Count < 1)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        "아이디 혹은 비밀번호가 맞지 않습니다."); // try문 밖으로 던짐
                }
                var selectUser = select.First();

                rv.StatusCode = EStatusCode.OK;
                rv.Message = "";
                rv.IsSuccess = true;
                rv.Data = selectUser;

            }
            catch (CommonException ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.Data = null;
                return rv;
            }
            catch (Exception ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
                rv.Data = ex.Data as ResDtoSearchUser;

                return rv;
            }
            return rv;
        }

        [HttpPost("FindAccountPassword")]
        public async Task<CommonResult<ResDtoFindAccountPassword>> 
            FindAccountPassword([FromBody] ReqDtoFindAccountPassword requestDto)
        {
            CommonResult<ResDtoFindAccountPassword> rv = new();

            try
            {
                rv.Data = new();

                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.UserName == requestDto.UserName)
                            select new ResDtoFindAccountPassword
                            {
                                Password = user.Password
                            }).ToListAsync();


                if (select.Count < 1)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        "아이디가 존재하지 않습니다."); // try문 밖으로 던짐
                }
                var selectUser = select.First();

                rv.StatusCode = EStatusCode.OK;
                rv.Message = "";
                rv.IsSuccess = true;
                rv.Data = selectUser;

            }
            catch (CommonException ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.Data = null;
                return rv;
            }
            catch (Exception ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
                rv.Data = ex.Data as ResDtoFindAccountPassword;

                return rv;
            }
            return rv;
        }

        [HttpPost("UpdateAccountPassword")]
        public async Task<CommonResult<ResDtoUpdateAccountPassword>>
            UpdateAccountPassword([FromBody] ReqDtoUpdateAccountPassword requestDto)
        {
            CommonResult<ResDtoUpdateAccountPassword> rv = new();
            
            try
            {
                var userAccount = _context.TblUserAccounts.
                                    Where
                                    (
                                    user => user.UserName == requestDto.UserName && user.Password == requestDto.Password
                                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"아이디 혹은 비밀번호가 맞지 않습니다. UserName : {requestDto.UserName} Password : {requestDto.Password}");
                }

                userAccount.Password = requestDto.UpdatePassword;
                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero, 
                        $"UserName : {requestDto.UserName},  UpdatePassword: {requestDto.UpdatePassword}");
                }
                else
                {
                    rv.IsSuccess = true;
                    rv.StatusCode = EStatusCode.OK;
                    rv.Data = null;
                }
            }
            catch (CommonException ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.Data = null;
                return rv;
            }
            catch (Exception ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
                rv.Data = null;

                return rv;
            }
            return rv;
        }

        [HttpPost("DeleteUserAccount")]
        public async Task<CommonResult<ResDtoDeleteUserAccount>> DeleteUserAccount([FromBody] ReqDtoDeleteUserAccount requestDto)
        {
            CommonResult<ResDtoDeleteUserAccount> rv = new();

            try
            {
                var userAccount = _context.TblUserAccounts.
                    Where
                    (
                    user => user.UserName == requestDto.UserName && user.Password == requestDto.Password
                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity, 
                        $"아이디 혹은 비밀번호가 맞지 않습니다. UserName : {requestDto.UserName} Password : {requestDto.Password}");
                }

                _context.TblUserAccounts.Remove(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero, $"UserName : {requestDto.UserName}");
                }
                else
                {
                    rv.IsSuccess = true;
                    rv.StatusCode = EStatusCode.OK;
                    rv.Data = null;
                }
            }
            catch (CommonException ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.Data = null;
            }
            catch (Exception ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
                rv.Data = null;
            }

            return rv;
        }
    }
}
