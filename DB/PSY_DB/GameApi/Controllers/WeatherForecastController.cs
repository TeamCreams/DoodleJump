using GameApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PSY_DB;
using PSY_DB.Tables;
using System.Text.Json.Serialization;

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

        [HttpGet("GetWeatherForecast2")]
        public async Task<string> Get2()
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
        [HttpPost("UserAdd")]
        public async Task<ResDtoAddUser> UserAdd([FromBody]ReqDtoAddUser requestDto)
        {
            ResDtoAddUser rv = new ();

            TblUserAccount userAccount = new();
            userAccount.RegisterDate = DateTime.Now;
            userAccount.UpdateDate = DateTime.Now;
            userAccount.UserName = requestDto.UserName;
            userAccount.Password = requestDto.Password;

            _context.TblUserAccounts.Add(userAccount);
            await _context.SaveChangesAsync();
            return rv;
        }
    }
}
