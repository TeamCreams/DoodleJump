using GameApi.Dtos;
using GameApi.Services;
using GameApiDto.Dtos;
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
    public class CashController : ControllerBase
    {
        private readonly ILogger<CashController> _logger;
        private readonly PsyDbContext _context;
        private readonly CashService _service;


        public CashController(ILogger<CashController> logger, PsyDbContext context, CashService service)
        {
            _logger = logger;
            _context = context;
            _service = service;
        }

        [HttpPost("AddCashProduct")]
        public async Task<CommonResult<ResDtoAddCashProduct>> AddCashProduct([FromBody] ReqDtoAddCashProduct request)
        {
            CommonResult<ResDtoAddCashProduct> rv = new();

            try
            {
                rv.Data = await _service.AddCashProduct(request);

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success";
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

        [HttpGet("GetCashProductList")]
        public async Task<CommonResult<ResDtoGetCashProductList>> GetCashProductList([FromQuery] ReqDtoGetCashProductList request)
        {
            CommonResult<ResDtoGetCashProductList> rv = new();

            try
            {
                rv.Data = await _service.GetCashProductList(request);

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success";
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

        [HttpPost("DeleteCashProduct")]
        public async Task<CommonResult<ResDtoDeleteCashProduct>> DeleteCashProduct([FromBody] ReqDtoDeleteCashProduct request)
        {
            CommonResult<ResDtoDeleteCashProduct> rv = new();

            try
            {
                rv.Data = await _service.DeleteCashProduct(request);

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success";
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


        [HttpPost("PurchaseCashProduct")]
        public async Task<CommonResult<ResDtoPurchaseCashProduct>> PurchaseCashProduct([FromBody] ReqDtoPurchaseCashProduct request)
        {
            CommonResult<ResDtoPurchaseCashProduct> rv = new();

            try
            {
                rv.Data = await _service.PurchaseCashProduct(request);

                rv.IsSuccess = true;
                rv.StatusCode = EStatusCode.OK;
                rv.Message = "Success";
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
