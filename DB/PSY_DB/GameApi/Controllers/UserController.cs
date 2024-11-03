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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly PsyDbContext _context;


        public UserController(ILogger<UserController> logger, PsyDbContext context)
        {
            _logger = logger;
            _context = context;
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
        [HttpPost("InsertUser")]
        public async Task<CommonResult<ResDtoInsertUserAccount>> InsertUser([FromBody] ReqDtoInsertUserAccount requestDto)
        {
            CommonResult<ResDtoInsertUserAccount> rv = new();

            try{ 
                var select = await (
                            from user in _context.TblUserAccounts
                            where user.UserName == requestDto.UserName 
                            select new
                            {
                                UserName = user.UserName
                            }).ToListAsync();

                if (select.Any() == false)
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

        [HttpGet("GetUserAccount")]
        public async Task<CommonResult<ResDtoGetUserAccount>> GetUserAccount([FromQuery] ReqDtoGetUserAccount requestDto)
        {
            CommonResult<ResDtoGetUserAccount> rv = new();

            try
            {
                rv.Data = new();

                var select = await (
                            from user in _context.TblUserAccounts.Include(user => user.TblUserScores)
                            where (user.UserName == requestDto.UserName && user.Password == requestDto.Password)
                            select new ResDtoGetUserAccount                            
                            {
                                UserName = user.UserName,
                                RegisterDate = user.RegisterDate,
                                UpdateDate = user.UpdateDate,
                                HighScore = user.TblUserScores
                                              .OrderByDescending(s => s.History)
                                              .Select(s => s.History)
                                              .FirstOrDefault()
                            }).ToListAsync();
                /*
                 *  SELECT `t`.`UserName`, `t`.`RegisterDate`, `t`.`UpdateDate`, COALESCE((
                          SELECT `t0`.`History`
                          FROM `TblUserScore` AS `t0`
                          WHERE `t`.`Id` = `t0`.`UserAccountId`
                          ORDER BY `t0`.`History` DESC
                          LIMIT 1), 0) AS `HighScore`
                      FROM `TblUserAccount` AS `t`
                      WHERE (`t`.`UserName` = @__requestDto_UserName_0) AND (`t`.`Password` = @__requestDto_Password_1)
                 */

                if (false == select.Any())
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
                rv.Data = ex.Data as ResDtoGetUserAccount;

                return rv;
            }
            return rv;
        }

        [HttpGet("GetUserAccountPassword")]
        public async Task<CommonResult<ResDtoGetUserAccountPassword>>
            GetUserAccountPassword([FromQuery] ReqDtoGetUserAccountPassword requestDto)
        {
            CommonResult<ResDtoGetUserAccountPassword> rv = new();

            try
            {
                rv.Data = new();

                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.UserName == requestDto.UserName &&
                                user.DeletedDate == null) // 삭제 되기 전이 null값
                            select new ResDtoGetUserAccountPassword
                            {
                                Password = user.Password
                            }).ToListAsync();


                if (select.Any() == false)
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
                rv.Data = ex.Data as ResDtoGetUserAccountPassword;

                return rv;
            }
            return rv;
        }

        [HttpPost("UpdateAccountPassword")]
        public async Task<CommonResult<ResDtoUpdateUserAccountPassword>>
            UpdateAccountPassword([FromBody] ReqDtoUpdateUserAccountPassword requestDto)
        {
            CommonResult<ResDtoUpdateUserAccountPassword> rv = new();
            
            try
            {
                var userAccount = _context.TblUserAccounts.
                                    Where
                                    (
                                        user => user.UserName == requestDto.UserName && 
                                            user.Password == requestDto.Password
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

                userAccount.DeletedDate = DateTime.UtcNow;

                //_context.TblUserAccounts.Remove(userAccount);

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

        [HttpGet("GetUserAccountId")]
        public async Task<CommonResult<ResDtoGetUserAccountId>> GetUserAccountId([FromQuery] ReqDtoGetUserAccountId requestDto)
        {
            CommonResult<ResDtoGetUserAccountId> rv = new();

            try
            {
                rv.Data = new();

                var select = await (from userAccount in _context.TblUserAccounts
                            where(userAccount.UserName == requestDto.UserName)//&& userAccount.DeletedDate == null)
                            select new
                            {
                                UserName = userAccount.UserName,
                            }).ToListAsync();
              
                if (true == select.Any())
                {
                    throw new CommonException(EStatusCode.NameAlreadyExists,
                        "사용할 수 없는 아이디입니다.");
                }
                else
                {
                    rv.StatusCode = EStatusCode.OK;
                    rv.Message = "";
                    rv.IsSuccess = true;
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
                rv.Data = ex.Data as ResDtoGetUserAccountId;

                return rv;
            }
            return rv;
        }
    }
}
