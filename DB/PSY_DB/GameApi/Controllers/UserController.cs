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
                        DeletedDate = userAccount.DeletedDate
                    }).ToListAsync();

            return JsonConvert.SerializeObject(a);
        }
        //Get - FromQuery
        //Post - FromBody
        [HttpPost("InsertUser")]
        public async Task<CommonResult<ResDtoInsertUserAccount>> InsertUser([FromBody] ReqDtoInsertUserAccount requestDto)
        {
            CommonResult<ResDtoInsertUserAccount> rv = new();

            //Thread.Sleep(3000);
            try
            { 
                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null)
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

            //Thread.Sleep(3000);
            try
            {
                rv.Data = new();

                var select = await (
                    from user in _context.TblUserAccounts.Include(user => user.TblUserScores)
                    where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null)
                    //where (user.UserName == requestDto.UserName && user.Password == requestDto.Password && user.DeletedDate == null)
                    select new ResDtoGetUserAccount
                    {
                        UserName = user.UserName,
                        Password = user.Password,
                        Nickname = user.Nickname,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores
                                      .OrderByDescending(s => s.History)
                                      .Select(s => s.History)
                                      .FirstOrDefault(),
                        LatelyScore = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.UpdateDate)
                                        .FirstOrDefault()
                                        .History : 0, // 최근 점수
                        Gold = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.UpdateDate)
                                        .FirstOrDefault()
                                        .Gold : 0
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

                if (select.Any() == false)
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

            //Thread.Sleep(3000);
            try
            {
                rv.Data = new();

                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null) // 삭제 되기 전이 null값
                            select new ResDtoGetUserAccountPassword
                            {
                                Password = user.Password
                            }).ToListAsync(); //.FirstOrDefault();가 안됨


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

            //Thread.Sleep(3000);
            try
            {
                var userAccount = _context.TblUserAccounts.
                                    Where
                                    (
                                        user => user.UserName.ToLower() == requestDto.UserName.ToLower() && 
                                                user.Password == requestDto.Password &&
                                                user.DeletedDate == null
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

            //Thread.Sleep(3000);
            try
            {
                var userAccount = _context.TblUserAccounts.
                                    Where
                                    (
                                        user => user.UserName.ToLower() == requestDto.UserName.ToLower() && user.Password == requestDto.Password
                                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity, 
                        $"아이디 혹은 비밀번호가 맞지 않습니다. UserName : {requestDto.UserName} Password : {requestDto.Password}");
                }

                //userAccount.DeletedDate = DateTime.UtcNow;

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

        [HttpGet("GetValidateUserAccountId")]
        public async Task<CommonResult<ResDtoGetValidateUserAccountId>> GetValidateUserAccountId([FromQuery] ReqDtoGetValidateUserAccountId requestDto)
        {
            CommonResult<ResDtoGetValidateUserAccountId> rv = new();

            //Thread.Sleep(3000);

            try
            {
                rv.Data = new();

                var select = await (from user in _context.TblUserAccounts
                            where(user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null)
                            select new
                            {
                                UserName = user.UserName,
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
                rv.Data = ex.Data as ResDtoGetValidateUserAccountId;

                return rv;
            }
            return rv;
        }

        [HttpGet("GetValidateUserAccountNickname")]
        public async Task<CommonResult<ResDtoGetValidateUserAccountNickname>> 
            GetValidateUserAccountNickname([FromQuery] ReqDtoGetValidateUserAccountNickname requestDto)
        {
            CommonResult<ResDtoGetValidateUserAccountNickname> rv = new();

            //Thread.Sleep(3000);

            try
            {
                rv.Data = new();

                var select = await (from user in _context.TblUserAccounts
                                    where (user.Nickname.ToLower() == requestDto.Nickname.ToLower() && user.DeletedDate == null)
                                    select new
                                    {
                                        Nickname = user.Nickname,
                                    }).ToListAsync();

                if (true == select.Any())
                {
                    throw new CommonException(EStatusCode.NameAlreadyExists,
                        "사용할 수 없는 Nickname입니다.");
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
                rv.Data = ex.Data as ResDtoGetValidateUserAccountNickname;

                return rv;
            }
            return rv;
        }

        [HttpPost("InsertUserAccountScore")]
        public async Task<CommonResult<ResDtoInsertUserAccountScore>>
            InsertUserAccountScore([FromBody] ReqDtoInsertUserAccountScore requestDto)
        {
            CommonResult<ResDtoInsertUserAccountScore> rv = new();
            
            //Thread.Sleep(3000);
            try
            {
                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null)
                            select user.Id
                            ).ToListAsync();

                if (select.Any() == false)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"UserName : {requestDto.UserName}");
                }

                int userId = select.First();

                TblUserScore userScore = new TblUserScore
                {
                    UserAccountId = userId,
                    History = requestDto.Score,
                    Gold = requestDto.Gold,
                    UpdateDate = DateTime.Now
                };

                _context.TblUserScores.Add(userScore);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserName : {requestDto.UserName}");
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

        [HttpPost("InsertUserAccountNickname")]
        public async Task<CommonResult<ResDtoInsertUserAccountNickname>>
            InsertUserAccountNickname([FromBody] ReqDtoInsertUserAccountNickname requestDto)
        {
            CommonResult<ResDtoInsertUserAccountNickname> rv = new();

            //Thread.Sleep(3000);
            try
            {

                var select = await (from user in _context.TblUserAccounts
                                    where (user.Nickname.ToLower() == requestDto.Nickname.ToLower() && user.DeletedDate == null)
                                    select new
                                    {
                                        Nickname = user.Nickname,
                                    }).ToListAsync();

                if (true == select.Any())
                {
                    throw new CommonException(EStatusCode.NameAlreadyExists,
                        $"{requestDto.Nickname} : 사용할 수 없는 Nickname");
                }

                var userAccount = _context.TblUserAccounts.
                                    Where
                                    (
                                        user => user.UserName.ToLower() == requestDto.UserName.ToLower() &&
                                                user.DeletedDate == null
                                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                      $"{requestDto.UserName} : 찾을 수 없는 UserName");
                }

                userAccount.Nickname = requestDto.Nickname;
                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero, $"Insert Nickname : {requestDto.Nickname}");
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
                rv.Data = null;

                return rv;
            }
            return rv;
        }

        [HttpGet("GetOrAddUserAccount")]
        public async Task<CommonResult<ResDtoGetOrAddUserAccount>> GetOrAddUserAccount()
        {
            CommonResult<ResDtoGetOrAddUserAccount> rv = new();
            ReqDtoGetOrAddUserAccount requestDto = new(); 

            //Thread.Sleep(3000);
            try
            {
                requestDto.UserName = HttpContext.Connection.RemoteIpAddress?.ToString();
                
                rv.Data = new();

                var select = await (
                    from user in _context.TblUserAccounts.Include(user => user.TblUserScores)
                    where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null)
                    select new ResDtoGetOrAddUserAccount
                    {
                        UserName = user.UserName,
                        Nickname = user.Nickname,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores
                                      .OrderByDescending(s => s.History)
                                      .Select(s => s.History)
                                      .FirstOrDefault(),
                        LatelyScore = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.UpdateDate)
                                        .FirstOrDefault()
                                        .History : 0, // 최근 점수
                        Gold = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.UpdateDate)
                                        .FirstOrDefault()
                                        .Gold : 0
                    }).ToListAsync();

                if (select.Any() == false)
                {
                    TblUserAccount userAccount = new()
                    {
                        UserName = requestDto.UserName,
                        RegisterDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    _context.TblUserAccounts.Add(userAccount);
                }
                else
                {
                    var selectUser = select.First();
                    rv.StatusCode = EStatusCode.OK;
                    rv.Message = $"{requestDto.UserName} 계정 정보";
                    rv.IsSuccess = true;
                    rv.Data = selectUser;
                    return rv;
                }

                var saveResult = await _context.SaveChangesAsync();

                if (saveResult == 0)
                {
                    throw new CommonException(EStatusCode.NameAlreadyExists, $"{requestDto.UserName} : Already Exists");
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

        [HttpGet("GetUserAccountList")]
        public async Task<CommonResult<ResDtoGetUserAccountList>> GetUserAccountList()
        {
            CommonResult<ResDtoGetUserAccountList> rv = new();

            try
            {
                rv.Data = new();
                rv.Data.List = await (from user in _context.TblUserAccounts.Include(user => user.TblUserScores)
                                where (user.Nickname != null && user.DeletedDate == null)
                                select new ResDtoGetUserAccountListElement
                                {
                                    UserName = user.UserName,
                                    Nickname = user.Nickname,
                                    HighScore = user.TblUserScores
                                            .OrderByDescending(s => s.History)
                                            .Select(s => s.History)
                                            .FirstOrDefault()
                                }).OrderByDescending(u => u.HighScore)
                                .ToListAsync();

                if (rv.Data.List.Any() == false)
                {
                    rv.StatusCode = EStatusCode.NotFoundEntity;
                    rv.Message = "rv.Data.List.Any() == false";
                    rv.IsSuccess = true;
                    rv.Data.List = null;
                    return rv;
                }
                rv.Message = "success load";
                rv.IsSuccess = true;
                return rv;
            }
            catch (CommonException ex)
            {
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.IsSuccess = false;
                rv.Data.List = null;
                return rv;
            }
            catch (Exception ex)
            {
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.ToString();
                rv.IsSuccess = false;
                rv.Data.List = null;
                return rv;
            }
            return rv;
        }

    [HttpPost("InsertUserMissions")]
    public async Task<CommonResult<ResDtoInsertUserMissions>> InsertUserMissions([FromBody] ReqDtoInsertUserMissions requestDto)
    {
        CommonResult<ResDtoInsertUserMissions> rv = new();

        //Thread.Sleep(3000);
        try
        {
            var select = await (
                        from user in _context.TblUserAccounts
                        where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null)
                        select new
                        {
                            UserName = user.UserName
                        }).ToListAsync();



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
    }
}
