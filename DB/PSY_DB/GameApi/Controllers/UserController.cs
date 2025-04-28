using GameApi.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PSY_DB;
using PSY_DB.Tables;
using random_alphanumeric_strings;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
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

                    userAccount.UserName = requestDto.UserName;
                    userAccount.Password = requestDto.Password;
                    userAccount.Nickname = requestDto.NickName;
                    userAccount.HairStyle = "Afro";
                    userAccount.EyebrowStyle = "AnnoyedEyebrows";
                    userAccount.EyesStyle = "Annoyed";
                    userAccount.RegisterDate = DateTime.UtcNow;
                    userAccount.UpdateDate = DateTime.UtcNow;
                    //userAccount.Energy = 10;
                    userAccount.LatelyEnergy = DateTime.UtcNow;
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

        //게임 시작/로그인 할 때 가장 먼저 사용할 것
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
                    where (
                          // 일반 로그인 케이스
                          (user.UserName.ToLower() == requestDto.UserName.ToLower() &&
                           user.Password == requestDto.Password)
                        ) && user.DeletedDate == null
                    select new ResDtoGetUserAccount
                    {
                        UserAccountId = user.Id,
                        UserName = user.UserName,
                        Password = user.Password,
                        Nickname = user.Nickname,
                        GoogleAccount = user.GoogleAccount,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores.Any() 
                                            ? user.TblUserScores
                                            .OrderByDescending(s => s.Scoreboard)
                                            .Select(s => s.Scoreboard)
                                            .FirstOrDefault() : 0,
                        LatelyScore = user.TblUserScores.Any()
                                            ? user.TblUserScores
                                            .Where(s => 0 < s.Scoreboard)  // 0보다 큰 점수만 필터링
                                            .OrderByDescending(s => s.RegisterDate) // 최신 점수 순으로 정렬
                                            .Select(s => s.Scoreboard)
                                            .FirstOrDefault() : 0, // 0이 아닌 가장 최근 점수, 없으면 0 반환
                        Gold = user.Gold,
                        PlayTime = user.TblUserScores.Any() ? 
                                    user.TblUserScores.Sum(s => s.PlayTime) : 0,
                        AccumulatedStone = user.TblUserScores.Any() ? user.TblUserScores
                                    .Sum(s => s.AccumulatedStone) : 0,
                        StageLevel = user.TblUserScores.Any() ? user.TblUserScores.Max(s => s.StageLevel) : 0,
                        CharacterId = user.CharacterId,
                        HairStyle = user.HairStyle,
                        EyesStyle = user.EyesStyle,
                        EyebrowStyle = user.EyebrowStyle,
                        Evolution = user.Evolution,
                        EvolutionSetLevel = user.EvolutionSetLevel,
                        LatelyEnergy = user.LatelyEnergy,
                        Energy = user.Energy,
                        PurchaseEnergyCountToday = (user.FirstPurchaseEnergyTime == DateTime.MinValue ||
                                     user.FirstPurchaseEnergyTime.AddHours(24) <= DateTime.UtcNow)
                                     ? 0
                                     : user.PurchaseEnergyCountToday,
                        LastRewardClaimTime = user.LastRewardClaimTime
                    }).ToListAsync();

                if (select.Any() == false)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        "아이디 혹은 비밀번호가 맞지 않습니다."); // try문 밖으로 던짐
                }
                var selectUser = select.First();

                //if (selectUser.LatelyScore == -1)
                //{
                //    selectUser.LatelyScore = 0; // -1일 경우 0으로 설정
                //}

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
                rv.Data = null;
                return rv;
            }
            return rv;
        }

        //게임 시작/로그인 할 때 가장 먼저 사용할 것
        [HttpGet("GetUserAccountByGoogle")]
        public async Task<CommonResult<ResDtoGetUserAccountByGoogle>> GetUserAccountByGoogle([FromQuery] ReqDtoGetUserAccountByGoogle requestDto)
        {
            CommonResult<ResDtoGetUserAccountByGoogle> rv = new();

            //Thread.Sleep(3000);
            try
            {
                rv.Data = new();

                var select = await (
                    from user in _context.TblUserAccounts.Include(user => user.TblUserScores)
                    where (
                          // 구글 로그인 케이스
                          (user.GoogleAccount == requestDto.GoogleAccount)
                        ) && user.DeletedDate == null
                    select new ResDtoGetUserAccountByGoogle
                    {
                        UserAccountId = user.Id,
                        UserName = user.UserName,
                        Password = user.Password,
                        Nickname = user.Nickname,
                        GoogleAccount = user.GoogleAccount,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores.Any()
                                        ? user.TblUserScores
                                        .OrderByDescending(s => s.Scoreboard)
                                        .Select(s => s.Scoreboard)
                                        .FirstOrDefault() : 0,
                        LatelyScore = user.TblUserScores.Any()
                                            ? user.TblUserScores
                                            .Where(s => 0 < s.Scoreboard)  // 0보다 큰 점수만 필터링
                                            .OrderByDescending(s => s.RegisterDate) // 최신 점수 순으로 정렬
                                            .Select(s => s.Scoreboard)
                                            .FirstOrDefault() : 0, // 0이 아닌 가장 최근 점수, 없으면 0 반환
                        Gold = user.Gold,
                        PlayTime = user.TblUserScores.Any() ?
                                    user.TblUserScores.Sum(s => s.PlayTime) : 0,
                        AccumulatedStone = user.TblUserScores.Any() ? user.TblUserScores
                                    .Sum(s => s.AccumulatedStone) : 0,
                        StageLevel = user.TblUserScores.Any() ? user.TblUserScores.Max(s => s.StageLevel) : 0,
                        CharacterId = user.CharacterId,
                        HairStyle = user.HairStyle,
                        EyesStyle = user.EyesStyle,
                        EyebrowStyle = user.EyebrowStyle,
                        Evolution = user.Evolution,
                        EvolutionSetLevel = user.EvolutionSetLevel,
                        LatelyEnergy = user.LatelyEnergy,
                        Energy = user.Energy,
                        PurchaseEnergyCountToday = (user.FirstPurchaseEnergyTime == DateTime.MinValue ||
                                     user.FirstPurchaseEnergyTime.AddHours(24) <= DateTime.UtcNow)
                                     ? 0
                                     : user.PurchaseEnergyCountToday,
                        LastRewardClaimTime = user.LastRewardClaimTime
                    }).ToListAsync();

                if (select.Any() == false)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        "google Account가 맞지 않습니다."); // try문 밖으로 던짐
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
                rv.Data = null;
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

                var select = await (from user in _context.TblUserAccounts
                                    where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null) // 삭제 되기 전이 null값
                                    select new ResDtoGetUserAccountPassword
                                    {
                                        Password = user.Password
                                    }).FirstOrDefaultAsync();

                if (select == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        "아이디가 존재하지 않습니다."); // try문 밖으로 던짐                }
                }

                //var select = await (
                //            from user in _context.TblUserAccounts
                //            where (user.UserName.ToLower() == requestDto.UserName.ToLower() && user.DeletedDate == null) // 삭제 되기 전이 null값
                //            select new ResDtoGetUserAccountPassword
                //            {
                //                Password = user.Password
                //            }).ToListAsync(); //.FirstOrDefault();가 안됨


                //if (select.Any() == false)
                //{
                //    throw new CommonException(EStatusCode.NotFoundEntity,
                //        "아이디가 존재하지 않습니다."); // try문 밖으로 던짐
                //}
                //var selectUser = select.First();

                rv.StatusCode = EStatusCode.OK;
                rv.Message = "";
                rv.IsSuccess = true;
                rv.Data = select;

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
                                                user.Password == requestDto.Password && //requestDto.Password 는 0000
                                                user.DeletedDate == null
                                    ).FirstOrDefault();

                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"아이디 혹은 비밀번호가 맞지 않습니다. UserAccountId : {requestDto.UserName} Password : {requestDto.Password}");
                }
                
                userAccount.Password = requestDto.UpdatePassword;
                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero, 
                        $"UserAccountId : {requestDto.UserName},  UpdatePassword: {requestDto.UpdatePassword}");
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
                        $"아이디 혹은 비밀번호가 맞지 않습니다. UserAccountId : {requestDto.UserName} Password : {requestDto.Password}");
                }

                //userAccount.DeletedDate = DateTime.UtcNow;

                _context.TblUserAccounts.Remove(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero, $"UserAccountId : {requestDto.UserName}");
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

        [HttpPost("CheckUserAccountUserNameExists")]
        public async Task<CommonResult<ResDtoUserAccountUserName>> CheckUserAccountUserNameExists([FromBody] ReqDtoUserAccountUserName requestDto)
        {
            CommonResult<ResDtoUserAccountUserName> rv = new();

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
                    rv.Message = "사용할 수 있는 아이디입니다.";
                    rv.IsSuccess = true;
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
                rv.Data = ex.Data as ResDtoUserAccountUserName;

                return rv;
            }
            return rv;
        }

        [HttpPost("CheckUserAccountNicknameExists")]
        public async Task<CommonResult<ResDtoUserAccountNickname>>
            CheckUserAccountNicknameExists([FromBody] ReqDtoUserAccountNickname requestDto)
        {
            CommonResult<ResDtoUserAccountNickname> rv = new();

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
                        "사용할 수 없는 Nickname입니다.");
                }
                else
                {
                    rv.StatusCode = EStatusCode.OK;
                    rv.Message = "";
                    rv.IsSuccess = true;
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
                rv.Data = ex.Data as ResDtoUserAccountNickname;

                return rv;
            }
            return rv;
        }

        [HttpPost("CheckGoogleAccountExists")]
        public async Task<CommonResult<ResDtoGoogleAccount>> CheckGoogleAccountExists([FromBody] ReqDtoGoogleAccount requestDto)
        {
            CommonResult<ResDtoGoogleAccount> rv = new();

            try
            {
                var select = await (from user in _context.TblUserAccounts
                                    where (user.GoogleAccount == requestDto.GoogleAccount && user.DeletedDate == null)
                                    select new
                                    {
                                        GoogleAccount = user.GoogleAccount,
                                    }).ToListAsync();

                if (select.Any())
                {
                    rv.StatusCode = EStatusCode.GoogleAccountAlreadyExists;
                    rv.Message = "사용할 수 없는 GoogleAccount.";
                    rv.IsSuccess = false;
                }
                else
                {
                    rv.StatusCode = EStatusCode.OK;
                    rv.Message = "";
                    rv.IsSuccess = true;
                }
                return rv;
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
                rv.Data = null; // ex.Data는 ResDtoGoogleAccount로 변환할 수 없음
                return rv;
            }
        }

        [HttpPost("InsertGoogleAccount")]
        public async Task<CommonResult<ResDtoInsertGoogleAccount>>
            InsertGoogleAccount([FromBody] ReqDtoInsertGoogleAccount requestDto)
        {
            CommonResult<ResDtoInsertGoogleAccount> rv = new();

            try
            {
                // 기존에 동일한 구글 계정이 있는지 확인
                var select = await (
                            from user in _context.TblUserAccounts
                            where (user.GoogleAccount == requestDto.GoogleAccount && user.DeletedDate == null)
                            select new
                            {
                                GoogleAccount = user.GoogleAccount
                            }).ToListAsync();

                // 기존 계정이 없으면 새로운 사용자 등록
                if (select.Any() == false)
                {
                    TblUserAccount userAccount = new();

                    userAccount.GoogleAccount = requestDto.GoogleAccount;
                    userAccount.Nickname = requestDto.NickName;
                    userAccount.HairStyle = "Afro";
                    userAccount.EyebrowStyle = "AnnoyedEyebrows";
                    userAccount.EyesStyle = "Annoyed";
                    userAccount.RegisterDate = DateTime.UtcNow;
                    userAccount.UpdateDate = DateTime.UtcNow;
                    userAccount.LatelyEnergy = DateTime.UtcNow;
                    _context.TblUserAccounts.Add(userAccount);
                }

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"GoogleAccount : {requestDto.GoogleAccount}");
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

        [HttpPost("BindUserAccountToGoogle")]
        public async Task<CommonResult<ResDtoBindUserAccountToGoogle>>
            BindUserAccountToGoogle([FromBody] ReqDtoBindUserAccountToGoogle requestDto)
        {
            CommonResult<ResDtoBindUserAccountToGoogle> rv = new();

            try
            {
                var userAccount = _context.TblUserAccounts.
                                    Where
                                    (
                                        user => user.Id == requestDto.UserAccountId &&
                                                user.GoogleAccount != requestDto.GoogleAccount &&
                                                user.DeletedDate == null
                                    ).FirstOrDefault();

                if(userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"UserId : {requestDto.UserAccountId} or Already used Google account : {requestDto.GoogleAccount}");
                }

                userAccount.GoogleAccount = requestDto.GoogleAccount;

                _context.TblUserAccounts.Update(userAccount);
                
                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserId : {requestDto.GoogleAccount}");
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


        [HttpPost("InsertUserAccountScore")]
        public async Task<CommonResult<ResDtoInsertUserAccountScore>>
            InsertUserAccountScore([FromBody] ReqDtoInsertUserAccountScore requestDto)
        {
            CommonResult<ResDtoInsertUserAccountScore> rv = new();
            
            try
            {
                var select = await (from user in _context.TblUserAccounts
                                    where (user.Id == requestDto.UserAccountId &&
                                        user.DeletedDate == null)
                                    select user
                                    ).FirstOrDefaultAsync();

                if (select == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                       $"UserId : {requestDto.UserAccountId}");
                }

                select.Gold += requestDto.Gold;
                TblUserScore userScore = new TblUserScore
                {
                    UserAccountId = select.Id,
                    Scoreboard = requestDto.Score,
                    PlayTime = requestDto.Time,
                    AccumulatedStone = requestDto.AccumulatedStone,
                    StageLevel = requestDto.StageLevel,
                    Gold = select.Gold,
                    RegisterDate = DateTime.UtcNow
                };
                _context.TblUserAccounts.Update(select);
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

        // XXX
        [HttpPost("InsertUserAccountNickname")]
        public async Task<CommonResult<ResDtoInsertUserAccountNickname>>
            InsertUserAccountNickname([FromBody] ReqDtoInsertUserAccountNickname requestDto)
        {
            CommonResult<ResDtoInsertUserAccountNickname> rv = new();

            //Thread.Sleep(3000);
            try
            {

                var select = await (from user in _context.TblUserAccounts
                                    where (user.Id == requestDto.UserAccountId &&
                                        user.DeletedDate == null &&
                                        user.Nickname == requestDto.Nickname)
                                    select user
                                    ).FirstOrDefaultAsync();

                if (select != null)
                {
                    throw new CommonException(EStatusCode.NameAlreadyExists,
                        $"해당 닉네임을 사용하는 계정이 이미 존재함.");
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

        // XXX
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
                        UserAccountId = user.Id,
                        UserName = user.UserName,
                        Nickname = user.Nickname,
                        GoogleAccount = user.GoogleAccount,
                        RegisterDate = user.RegisterDate,
                        UpdateDate = user.UpdateDate,
                        HighScore = user.TblUserScores
                                      .OrderByDescending(s => s.Scoreboard)
                                      .Select(s => s.Scoreboard)
                                      .FirstOrDefault(),
                        LatelyScore = user.TblUserScores
                                        //.Where(s => s.Scoreboard != -1) // -1이 아닌 점수만 ( -1은 Gold만 받았을 때 들어가는 점수)
                                        .OrderByDescending(s => s.RegisterDate) // 최신 점수 순으로 정렬
                                        .Select(s => s.Scoreboard)
                                        .FirstOrDefault(), // 가장 최근 점수
                        Gold = user.TblUserScores.Any() ?
                                        user.TblUserScores.OrderByDescending(s => s.RegisterDate)
                                        .FirstOrDefault()
                                        .Gold : 0,
                        PlayTime = user.TblUserScores
                                        .Sum(s => s.PlayTime),
                        AccumulatedStone = user.TblUserScores
                                    //.Where(s => s.AccumulatedStone != -1)
                                    .Sum(s => s.AccumulatedStone),
                        StageLevel = user.TblUserScores
                                    .Max(s => s.StageLevel),
                        CharacterId = user.CharacterId,
                        HairStyle = user.HairStyle,
                        EyesStyle = user.EyesStyle,
                        EyebrowStyle = user.EyebrowStyle,
                        Evolution = user.Evolution,
                        EvolutionSetLevel = user.EvolutionSetLevel,
                        LatelyEnergy = user.LatelyEnergy,
                        Energy = user.Energy,
                        PurchaseEnergyCountToday = (user.FirstPurchaseEnergyTime == DateTime.MinValue ||
                                     user.FirstPurchaseEnergyTime.AddHours(24) <= DateTime.UtcNow)
                                     ? 0
                                     : user.PurchaseEnergyCountToday,
                        LastRewardClaimTime = user.LastRewardClaimTime
                    }).ToListAsync();

                if (select.Any() == false)
                {
                    TblUserAccount userAccount = new()
                    {
                        UserName = requestDto.UserName,
                        RegisterDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                    _context.TblUserAccounts.Add(userAccount);
                }
                else
                {
                    var selectUser = select.First();

                    //if (selectUser.LatelyScore == -1)
                    //{
                    //    selectUser.LatelyScore = 0; // -1일 경우 0으로 설정
                    //}

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
                                    UserAccountId = user.Id,
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

                // 아이템 값만큼 골드 뺏기
                userAccount.Gold -= requestDto.Gold;
                userAccount.UpdateDate = DateTime.UtcNow;

                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();

                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
                }
                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success UpdateGold";

                rv.Data = new ResDtoUpdateUserGold
                {
                    Gold = userAccount.Gold
                };
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
        #region Reward
        [HttpPost("UpdateRewardClaim")]
        public async Task<CommonResult<ResDtoUpdateRewardClaim>> UpdateRewardClaim([FromBody] ReqDtoUpdateRewardClaim requestDto)
        {
            CommonResult<ResDtoUpdateRewardClaim> rv = new();
            try
            {
                rv.Data = new();

                var userAccount = await _context.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null);
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                if (userAccount.LastRewardClaimTime == DateTime.MinValue
                    || userAccount.LastRewardClaimTime.AddHours(24) <= DateTime.UtcNow) // 24시간이 지났으면 리셋
                {
                    userAccount.LastRewardClaimTime = DateTime.UtcNow;
                    userAccount.Gold += requestDto.Gold;
                }
                
                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();
                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
                }
                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success UpdateRewardClaim";

                //energy 수정 
                rv.Data = new ResDtoUpdateRewardClaim
                {
                    Gold = userAccount.Gold,
                    LastRewardClaimTime = userAccount.LastRewardClaimTime
                };
            }
            catch (CommonException ex)
            {
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.IsSuccess = false;
                return rv;
            }
            catch (Exception ex)
            {
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.ToString();
                rv.IsSuccess = false;
                return rv;
            }
            return rv;
        }
        #endregion
        #region Quest
        //퀘스트 수락
        [HttpPost("InsertUserMissionList")]
        public async Task<CommonResult<ResDtoInsertUserMissionList>> InsertUserMissions([FromBody] ReqDtoInsertUserMissionList requestDto)
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
                        .Where(mission => mission.UserAccountId == requestDto.UserAccountId)
                        .Where(mission => requestedMissionIds.Contains(mission.MissionId))
                        .Select(mission => mission.MissionId)
                        .ToListAsync();

                foreach (var req in requestDto.List)
                {
                    //없는 것만 추가할 수 있도록
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
                // 이전과 변한게 없다면 패스
                if (requestDto.List.Any(req => !existingMissionIds.Contains(req.MissionId)))
                {
                    var isSuccess = await _context.SaveChangesAsync();

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
        public async Task<CommonResult<ResDtoCompleteUserMissionList>> CompleteUserMission([FromBody] ReqDtoCompleteUserMission requestDto)
        {
            CommonResult<ResDtoCompleteUserMissionList> rv = new();

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
                                         mission => mission.UserAccountId == userAccount.Id
                                         && mission.MissionId == requestDto.MissionId
                                     ).FirstOrDefault();
                if (userMission == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"찾을 수 없는 미션. MissionId : {requestDto.MissionId} Param1 : {requestDto.Param1}");
                }

                // 골드 추가
                userAccount.Gold += requestDto.Gold; 
                userAccount.UpdateDate = DateTime.UtcNow;
                _context.TblUserAccounts.Update(userAccount);

                // 미션 상태 : 보상 획득
                userMission.Param1 = requestDto.Param1;
                userMission.MissionStatus = EMissionStatus.Rewarded;
                _context.TblUserMissions.Update(userMission);

                var isSuccess = await _context.SaveChangesAsync();
                if (isSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}, UpdateMission: {requestDto.MissionId} 아무것도 달라지지 않았음");
                }

                rv.Data = new ResDtoCompleteUserMissionList
                {
                    List = await _context.TblUserMissions
                        .Where(mission => mission.UserAccountId == requestDto.UserAccountId)
                        .Select(mission => new ResDtoCompleteUserMissionListElement
                        {
                            MissionId = mission.MissionId,
                            MissionStatus = (int)mission.MissionStatus,
                            Param1 = mission.Param1
                        }).ToListAsync(),
                     Gold = userAccount.Gold
                };

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success Complete Mission";
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
        [HttpPost("UpdateUserMissionList")]
        public async Task<CommonResult<ResDtoUpdateUserMissionList>> UpdateUserMission([FromBody] ReqDtoUpdateUserMissionList requestDto)
        {
            CommonResult<ResDtoUpdateUserMissionList> rv = new();

            try
            {
                var userAccount = await _context.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null);
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                //업데이트할 미션 Id가 있는 경우에만 업데이트
                if (requestDto.List != null)
                {
                    //사용자 미션을 가져옴
                    var userMissions = await _context.TblUserMissions
                        .Where(m => m.UserAccountId == userAccount.Id)
                        .ToListAsync();

                    foreach (var missionElement in requestDto.List)
                    {
                        if (missionElement == null)
                        {
                            Console.WriteLine("missionElement는 null.");
                            continue;
                        }

                        var userMission = userMissions.FirstOrDefault(m => m.MissionId == missionElement.MissionId);

                        //해당 미션이 없으면 계속 진행
                        if (userMission == null)
                        {
                            Console.WriteLine($"userMission Id is X: {userMission.MissionId}");
                            continue;
                        }

                        if (userMission.Param1 != missionElement.Param1)
                        {
                            userMission.MissionStatus = (EMissionStatus)missionElement.MissionStatus;
                            userMission.Param1 = missionElement.Param1;
                            _context.TblUserMissions.Update(userMission);
                        }
                    }

                    var isSuccess = await _context.SaveChangesAsync();
                    if (isSuccess == 0)
                    {
                        throw new CommonException(EStatusCode.ChangedRowsIsZero, "미션 상태 변경에 실패했습니다.");
                        // 계속 여기로 들어옴
                    }
                }

                //미션 반환
                rv.Data = new ResDtoUpdateUserMissionList
                {
                    List = await _context.TblUserMissions
                        .Where(mission => mission.UserAccountId == userAccount.Id)
                        .Select(mission => new ResDtoUpdateUserMissionListElement
                        {
                            MissionId = mission.MissionId,
                            MissionStatus = (int)mission.MissionStatus,
                            Param1 = mission.Param1
                        }).ToListAsync()
                };

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success return Missions";
            }
            catch (CommonException ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.Data = null;
                Console.WriteLine($"CommonException: {ex.Message}");
            }
            catch (Exception ex)
            {
                rv.IsSuccess = false;
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.Message;
                rv.Data = null;
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return rv;
        }


        #endregion
        #region Style
        [HttpPost("UpdateUserStyle")]
        public async Task<CommonResult<ResDtoUpdateUserStyle>>
            UpdateUserStyle([FromBody] ReqDtoUpdateUserStyle requestDto)
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
                userAccount.EvolutionSetLevel = requestDto.EvolutionSetLevel;

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
        #region HeartBeat
        [HttpPost("HeartBeat")]
        public async Task<CommonResult<ResDtoHeartBeat>> HeartBeat([FromBody] ReqDtoHeartBeat requestDto)
        {
            CommonResult<ResDtoHeartBeat> rv = new();
            try
            {
                rv.Data = new();

                rv.Data.DateTime = DateTime.UtcNow; 
                rv.IsSuccess = true;
            }
            catch (CommonException ex)
            {
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.IsSuccess = false;
                return rv;
            }
            catch (Exception ex)
            {
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.ToString();
                rv.IsSuccess = false;
                return rv;
            }
            return rv;
        }

        [HttpPost("UpdateEnergy")]
        public async Task<CommonResult<ResDtoUpdateEnergy>> UpdateEnergy([FromBody] ReqDtoUpdateEnergy requestDto)
        {
            CommonResult<ResDtoUpdateEnergy> rv = new();
            try
            {
                rv.Data = new();
                
                var userAccount = await _context.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null);
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                var diffTime = DateTime.UtcNow - userAccount.LatelyEnergy;

                int count = (int)diffTime.TotalSeconds / 300;
                int prevEnergy = userAccount.Energy;
                int maxEnergy = 10; // 기본 최대 에너지 값

                // 에너지가 이미 10 이상인 경우
                if (maxEnergy <= userAccount.Energy)
                {
                    // 유료 구매로 10을 초과한 경우는 충전하지 않음
                    // 에너지가 이미 최대치이므로 변경 없음
                }
                // 현재 에너지 + 충전될 에너지가 10을 초과하는 경우
                else if (maxEnergy <= userAccount.Energy + count)
                {
                    userAccount.Energy = maxEnergy; // 최대값으로 설정
                }
                // 충전 후에도 최대값 미만인 경우
                else
                {
                    userAccount.Energy += count; // 충전된 에너지 추가
                }

                if (prevEnergy != userAccount.Energy)
                {
                    userAccount.LatelyEnergy = userAccount.LatelyEnergy.Add(diffTime);
                }

                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();
                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
                }
                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success GameStart";

                //energy 수정 
                rv.Data = new ResDtoUpdateEnergy
                {
                    Energy = userAccount.Energy,
                    LatelyEnergy = userAccount.LatelyEnergy
                };
            }
            catch (CommonException ex)
            {
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.IsSuccess = false;
                return rv;
            }
            catch (Exception ex)
            {
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.ToString();
                rv.IsSuccess = false;
                return rv;
            }
            return rv;
        }

        [HttpPost("GameStart")]
        public async Task<CommonResult<ResDtoGameStart>> GameStart([FromBody] ReqDtoGameStart requestDto)
        {
            CommonResult<ResDtoGameStart> rv = new();
            try
            {
                rv.Data = new();

                var userAccount = await _context.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null);
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                if (userAccount.Energy <= 0)
                {
                    throw new CommonException(EStatusCode.EnergyInsufficient, "에너지가 부족합니다.");
                }

                if(userAccount.Energy <= 10)//이게 맞나? (10 <= userAccount.Energy) 원래 이랬음 
                {
                    userAccount.LatelyEnergy = DateTime.UtcNow;
                }
                userAccount.Energy--;

                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();
                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
                }
                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success GameStart";

                rv.Data = new ResDtoGameStart
                {
                    Energy = userAccount.Energy,
                    LatelyEnergy = userAccount.LatelyEnergy
                };
            }
            catch (CommonException ex)
            {
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.IsSuccess = false;
                return rv;
            }
            catch (Exception ex)
            {
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.ToString();
                rv.IsSuccess = false;
                return rv;
            }
            return rv;
        }

        [HttpGet("InsertEnergy")]
        public async Task<CommonResult<ResDtoInsertEnergy>> InsertEnergy([FromQuery] ReqDtoInsertEnergy requestDto)
        {
            // 이미 구매했으면 가격을 조금 더 받도록 수정하는 기능이 있었으면 좋겠는데.
            // 그럼 첫구매 시간을 저장해둬야할듯.
            CommonResult<ResDtoInsertEnergy> rv = new();
            try
            {
                rv.Data = new();

                var userAccount = await _context.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == requestDto.UserAccountId && user.DeletedDate == null);
                if (userAccount == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{requestDto.UserAccountId} : 찾을 수 없는 UserAccountId");
                }

                userAccount.Energy += requestDto.Energy;

                // 현재 UTC 시간
                DateTime nowUtc = DateTime.UtcNow;

                // 오늘 자정 (UTC 00:00) 계산
                DateTime todayUtcMidnight = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, 0, 0, 0, DateTimeKind.Utc);

                // FirstPurchaseEnergyTime이 없거나 오늘 UTC 자정 이전이면 초기화
                if (userAccount.FirstPurchaseEnergyTime == DateTime.MinValue ||
                    userAccount.FirstPurchaseEnergyTime < todayUtcMidnight)
                {
                    userAccount.FirstPurchaseEnergyTime = nowUtc;
                    userAccount.PurchaseEnergyCountToday = 0;
                }

                userAccount.PurchaseEnergyCountToday++;

                _context.TblUserAccounts.Update(userAccount);

                var IsSuccess = await _context.SaveChangesAsync();
                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {requestDto.UserAccountId}");
                }
                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success GameStart";

                //energy 수정 
                rv.Data = new ResDtoInsertEnergy
                {
                    Energy = userAccount.Energy,
                    PurchaseEnergyCountToday = userAccount.PurchaseEnergyCountToday // 구매한 횟수에 따른 추가 금액 반환
                };
            }
            catch (CommonException ex)
            {
                rv.StatusCode = (EStatusCode)ex.StatusCode;
                rv.Message = ex.Message;
                rv.IsSuccess = false;
                return rv;
            }
            catch (Exception ex)
            {
                rv.StatusCode = EStatusCode.ServerException;
                rv.Message = ex.ToString();
                rv.IsSuccess = false;
                return rv;
            }
            return rv;
        }
        #endregion
    }
}
