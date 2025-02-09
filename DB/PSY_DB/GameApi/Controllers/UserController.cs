﻿using GameApi.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PSY_DB;
using PSY_DB.Tables;
using System.Reflection;
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

        #region UserAccount
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
                        UserAccountId = user.Id,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores
                                      .OrderByDescending(s => s.Scoreboard)
                                      .Select(s => s.Scoreboard)
                                      .FirstOrDefault(),
                        LatelyScore = user.TblUserScores
                                        .Where(s => s.Scoreboard != -1) // -1이 아닌 점수만 ( -1은 Gold만 받았을 때 들어가는 점수)
                                        .OrderByDescending(s => s.UpdateDate) // 최신 점수 순으로 정렬
                                        .Select(s => s.Scoreboard) // History 선택
                                        .FirstOrDefault(), // 가장 최근 점수
                        Gold = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.UpdateDate)
                                        .FirstOrDefault()
                                        .Gold : 0,
                        PlayTime = user.TblUserScores
                                    .Where(s => s.PlayTime != -1)
                                    .Sum(s => s.PlayTime),
                        AccumulatedStone = user.TblUserScores
                                    .Where(s => s.AccumulatedStone != -1)
                                    .Sum(s => s.AccumulatedStone),
                        CharacterId = user.CharacterId,
                        HairStyle = user.HairStyle,
                        EyesStyle = user.EyesStyle,
                        EyebrowStyle = user.EyebrowStyle,
                        Evolution = user.Evolution 
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
                
                if (selectUser.LatelyScore == -1)
                {
                    selectUser.LatelyScore = 0; // -1일 경우 0으로 설정
                }

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
                            where (user.Id == requestDto.UserAccountId && user.DeletedDate == null)
                            select user.Id
                            ).ToListAsync();

                if (select.Any() == false)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"UserId : {requestDto.UserAccountId}");
                }

                int userId = select.First();

                TblUserScore userScore = new TblUserScore
                {
                    UserAccountId = userId,
                    Scoreboard = requestDto.Score,
                    PlayTime = requestDto.Time,
                    AccumulatedStone = requestDto.AccumulatedStone,
                    Gold = requestDto.Gold,
                    UpdateDate = DateTime.Now
                };

                _context.TblUserScores.Add(userScore);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserId : {requestDto.UserAccountId}");
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
                                    where (user.Id == requestDto.UserAccountId && user.DeletedDate == null)
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
                                        user => user.Id == requestDto.UserAccountId &&
                                                user.DeletedDate == null
                                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                      $"{requestDto.UserAccountId} : 찾을 수 없는 UserId");
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
                        UserAccountId = user.Id,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores
                                      .OrderByDescending(s => s.Scoreboard)
                                      .Select(s => s.Scoreboard)
                                      .FirstOrDefault(),
                        LatelyScore = user.TblUserScores
                                        .Where(s => s.Scoreboard != -1) // -1이 아닌 점수만 ( -1은 Gold만 받았을 때 들어가는 점수)
                                        .OrderByDescending(s => s.UpdateDate) // 최신 점수 순으로 정렬
                                        .Select(s => s.Scoreboard) // History 선택
                                        .FirstOrDefault(), // 가장 최근 점수
                        Gold = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.UpdateDate)
                                        .FirstOrDefault()
                                        .Gold : 0,
                        PlayTime = user.TblUserScores
                                    .Where(s => s.PlayTime != -1)
                                    .Sum(s => s.PlayTime),
                        AccumulatedStone = user.TblUserScores
                                    .Where(s => s.AccumulatedStone != -1)
                                    .Sum(s => s.AccumulatedStone),
                        CharacterId = user.CharacterId,
                        HairStyle = user.HairStyle,
                        EyesStyle = user.EyesStyle,
                        EyebrowStyle = user.EyebrowStyle,
                        Evolution = user.Evolution
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

                    if (selectUser.LatelyScore == -1)
                    {
                        selectUser.LatelyScore = 0; // -1일 경우 0으로 설정
                    }

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
                                            .OrderByDescending(s => s.Scoreboard)
                                            .Select(s => s.Scoreboard)
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

        //Gold 업데이트
        [HttpPost("UpdateUserGold")]
        public async Task<CommonResult<ResDtoUpdateUserGold>>
            UpdateUserMission([FromBody] ReqDtoUpdateUserGold requestDto)
        {
            CommonResult<ResDtoUpdateUserGold> rv = new();

            try
            {
                var userAccount = _context.TblUserAccounts
                                    .Where(
                                        user => user.Id == requestDto.UserAccountId &&
                                                user.DeletedDate == null
                                    ).FirstOrDefault();
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                userAccount.Gold = requestDto.Gold;
                userAccount.UpdateDate = DateTime.Now;

                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
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
        #endregion

        #region Quest
        //퀘스트 수락
        [HttpGet("InsertUserMissionList")]
        public async Task<CommonResult<ResDtoInsertUserMissionList>> InsertUserMissions([FromQuery] ReqDtoInsertUserMissionList requestDto)
        {
            CommonResult<ResDtoInsertUserMissionList> rv = new();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userId = await _context.TblUserAccounts
                    .Where(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null)
                    .Select(user => user.Id)
                    .FirstOrDefaultAsync();

                if (userId == 0)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity, $"UserAccountId : {requestDto.UserAccountId}");
                }

                var requestedMissionIds = requestDto.List.Select(req => req.MissionId).ToList();
                var existingMissionIds = await _context.TblUserMissions
                    .Where(mission => requestedMissionIds.Contains(mission.MissionId))
                    .Select(mission => mission.MissionId)
                    .ToListAsync();

                foreach (var req in requestDto.List)
                {
                    // 없는 것만 추가할 수 있도록
                    if (!existingMissionIds.Contains(req.MissionId))
                    {
                        var userMission = new TblUserMission
                        {
                            UserAccountId = userId,
                            MissionId = req.MissionId
                        };
                        _context.TblUserMissions.Add(userMission);
                    }
                }

                var isSuccess = await _context.SaveChangesAsync();
                if(1 < existingMissionIds.Count)
                {
                    if (isSuccess == 0)
                    {
                        throw new CommonException(EStatusCode.ChangedRowsIsZero, "미션 추가에 실패했습니다.");
                    }
                }

                await transaction.CommitAsync();

                rv.Data = new ResDtoInsertUserMissionList
                {
                    List = await _context.TblUserMissions
                        .Where(mission => mission.UserAccountId == requestDto.UserAccountId)
                        .Select(mission => new ResDtoInsertUserMissionListElement
                        {
                            MissionId = mission.MissionId,
                            MissionStatus = (int)mission.MissionStatus,
                            Param1 = mission.Param1
                        }).ToListAsync()
                };

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success Insert Missions";
            }
            catch (CommonException ex)
            {
                await transaction.RollbackAsync();
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
            }
            return rv;
        }


        //퀘스트 완료
        [HttpPost("CompleteUserMission")]
        public async Task<CommonResult<ResDtoCompleteUserMission>>
            CompleteUserMission([FromBody] ReqDtoCompleteUserMission requestDto)
        {
            CommonResult<ResDtoCompleteUserMission> rv = new();

            try
            {
                var userAccount = _context.TblUserAccounts
                                    .Where(
                                        user => user.Id == requestDto.UserAccountId &&
                                                user.DeletedDate == null
                                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity, 
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                var userMission = _context.TblUserMissions
                                    .Where( 
                                            mission => mission.MissionId == requestDto.MissionId &&
                                                        mission.UserAccountId == userAccount.Id
                                    ).FirstOrDefault();

                if (userMission == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity, 
                        $"미션을 완료하지 못했습니다. MissionId : {requestDto.MissionId} Param1 : {requestDto.Param1}");
                }

                userMission.MissionStatus = EMissionStatus.Complete;


                TblUserScore userScore = new TblUserScore
                {
                    UserAccountId = requestDto.UserAccountId,
                    PlayTime = -1,
                    Scoreboard = -1,
                    AccumulatedStone = -1,
                    Gold = requestDto.Gold,
                    UpdateDate = DateTime.Now
                };

                _context.TblUserMissions.Update(userMission);
                _context.TblUserScores.Add(userScore);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId},  UpdateMission: {requestDto.MissionId} 아무것도 달라지지 않았음");
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

        //퀘스트 리스트 가져오기
        [HttpGet("GetUserMissionList")]
        public async Task<CommonResult<ResDtoGetUserMissionList>> GetUserMissionList([FromQuery] ReqDtoGetUserMissionList requestDto)
        {
            CommonResult<ResDtoGetUserMissionList> rv = new();

            try
            {                
                //사용자 계정 검증
                var userAccount = await _context.TblUserAccounts
                                    .Where(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null)
                                    .FirstOrDefaultAsync();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"없는 아이디 UserAccountId : {requestDto.UserAccountId}");
                }

                rv.Data = new();

                rv.Data.List = await _context.TblUserMissions
                                .Where(mission => mission.UserAccountId == requestDto.UserAccountId)
                                .Select(mission => new ResDtoGetUserMissionListElement
                                {
                                    MissionId = mission.MissionId,
                                    MissionStatus = (int)mission.MissionStatus,
                                    Param1 = mission.Param1
                                })
                                .ToListAsync();

                //rv.Data.List = await (from user in _context.TblUserAccounts.Include(user => user.TblUserMissions)
                //                      from mission in user.TblUserMissions
                //                      where (user.Id == requestDto.UserAccountId && user.DeletedDate == null)
                //                      select new ResDtoGetUserMissionListElement
                //                      {
                //                          MissionId = mission.MissionId,
                //                          MissionStatus = (int)mission.MissionStatus,
                //                          Param1 = mission.Param1
                //                      }).ToListAsync();

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

        //퀘스트 진척 상황 업데이트
        [HttpGet("UpdateUserMissionList")]
        public async Task<CommonResult<ResDtoUpdateUserMissionList>> UpdateUserMission([FromQuery] ReqDtoUpdateUserMissionList requestDto)
        {
            CommonResult<ResDtoUpdateUserMissionList> rv = new()
            {
                Data = new ResDtoUpdateUserMissionList() // rv.Data 초기화
            };

            try
            {
                //사용자 계정 검증
                var userAccount = await _context.TblUserAccounts
                    .FirstOrDefaultAsync(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null);

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity, $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                //업데이트할 미션 ID가 있는 경우에만 업데이트 수행
                if (requestDto.List != null && requestDto.List.Any())
                {
                    //업데이트할 미션 ID 리스트
                    List<int> missionIdsToUpdate = requestDto.List.Select(m => m.MissionId).ToList();

                    //사용자 미션을 가져옴
                    var userMissions = await _context.TblUserMissions
                        .Where(mission => missionIdsToUpdate.Contains(mission.MissionId))
                        .ToListAsync();

                    foreach (ReqDtoUpdateUserMissionListElement missionElement in requestDto.List)
                    {
                        var userMission = userMissions.FirstOrDefault(m => m.MissionId == missionElement.MissionId);

                        //해당 미션이 없으면 계속 진행
                        if (userMission == null)
                        {
                            continue;
                        }

                        try
                        {
                            //업데이트
                            userMission.Param1 = missionElement.Param1;
                            _context.TblUserMissions.Update(userMission);
                        }
                        catch (Exception ex)
                        {
                            //미션 업데이트 실패 시 에러를 남기고 계속 진행
                            Console.WriteLine($"MissionId: {missionElement.MissionId} 업데이트 실패: {ex.Message}");
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                //미션 반환
                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Data.List = await _context.TblUserMissions
                    .Where(mission => mission.UserAccountId == userAccount.Id)
                    .Select(mission => new ResDtoUpdateUserMissionListElement
                    {
                        MissionId = mission.MissionId,
                        MissionStatus = (int)mission.MissionStatus,
                        Param1 = mission.Param1
                    })
                    .ToListAsync();
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


        #endregion
        #region Style
        [HttpPost("UpdateUserStyle")]
        public async Task<CommonResult<ResDtoUpdateUserStyle>>
            InsertUserStyle([FromBody] ReqDtoUpdateUserStyle requestDto)
        {
            CommonResult<ResDtoUpdateUserStyle> rv = new();

            try
            {
                var userAccount = _context.TblUserAccounts
                                    .Where(
                                        user => user.Id == requestDto.UserAccountId &&
                                                user.DeletedDate == null
                                    ).FirstOrDefault();
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                userAccount.CharacterId = requestDto.CharacterId;
                userAccount.HairStyle = requestDto.HairStyle;
                userAccount.EyesStyle = requestDto.EyesStyle;
                userAccount.EyebrowStyle = requestDto.EyebrowStyle;
                userAccount.Evolution = requestDto.Evolution;

                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
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
        #endregion
    }
}
