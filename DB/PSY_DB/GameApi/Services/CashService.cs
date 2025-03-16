using GameApi.Controllers;
using GameApiDto.Dtos;
using Microsoft.EntityFrameworkCore;
using PSY_DB;
using PSY_DB.Tables;
using WebApi.Models.Dto;

namespace GameApi.Services
{
    public class CashService
    {
        private readonly ILogger<CashService> _logger;
        private readonly PsyDbContext _context;

        public CashService(ILogger<CashService> logger, PsyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ResDtoAddCashProduct> AddCashProduct(ReqDtoAddCashProduct request)
        {
            ResDtoAddCashProduct rv = new();
            if (string.IsNullOrWhiteSpace(request.ProductId))
            {
                throw new CommonException(EStatusCode.RequestError, "ProductId가 비어있습니다.");
            }
            if (request.Price <= 0)
            {
                throw new CommonException(EStatusCode.RequestError, "Price는 0보다 커야합니다.");
            }
            if (request.Amount <= 0)
            {
                throw new CommonException(EStatusCode.RequestError, "Amount는 0보다 커야합니다.");
            }

            _context.TblCashProducts.Add(new PSY_DB.Tables.TblCashProduct()
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Price = request.Price,
                Currency = (ECurrencyType)request.Currency,
                ItemType = (EItemType)request.ItemType,
                Amount = request.Amount,
                IsConsumable = request.IsConsumable,
            });

            await _context.SaveChangesAsync();

            return rv;
        }

        public async Task<ResDtoDeleteCashProduct> DeleteCashProduct(ReqDtoDeleteCashProduct request)
        {
            ResDtoDeleteCashProduct rv = new();
            if (string.IsNullOrWhiteSpace(request.ProductId))
            {
                throw new CommonException(EStatusCode.RequestError, "ProductId가 비어있습니다.");
            }

            var cashProducts = _context.TblCashProducts.Where(cp => cp.ProductId == request.ProductId && cp.DeletedDate == null).ToList();

            for(int i =0; i < cashProducts.Count; i++)
            {
                cashProducts[i].DeletedDate = DateTime.UtcNow;
            }

            _context.UpdateRange(cashProducts);

            await _context.SaveChangesAsync();

            return rv;
        }

        public async Task<ResDtoPurchaseCashProduct> PurchaseCashProduct(ReqDtoPurchaseCashProduct request)
        {
            ResDtoPurchaseCashProduct rv = new();
            if (request.CashProductId <= 0)
            {
                throw new CommonException(EStatusCode.RequestError, "CashProductId가 비어있습니다.");
            }
            if (request.UserAccountId <= 0)
            {
                throw new CommonException(EStatusCode.RequestError, "UserAccountId가 비어있습니다.");
            }
            if (request.Amount <= 0)
            {
                throw new CommonException(EStatusCode.RequestError, "Amount가 비어있습니다.");
            }

            _context.TblUserCashProducts.Add(new TblUserCashProduct()
            {
                ProductId = request.CashProductId,
                UserAccountId = request.UserAccountId,
                Amount = request.Amount,
            });

            await _context.SaveChangesAsync();

            return rv;
        }

        public async Task<ResDtoGetCashProductList> GetCashProductList(ReqDtoGetCashProductList request)
        {
            ResDtoGetCashProductList rv = new();

            rv.List = await _context.TblCashProducts
                .Where(cp => cp.DeletedDate == null)
                .OrderByDescending(cp => cp.RegisterDate)
                .Select(cp => new ResDtoGetCashProductListItem()
                {
                    Id = cp.Id,
                    ProductId = cp.ProductId,
                    ProductName = cp.ProductName,
                    Amount = cp.Amount,
                    Currency = (ResDtoGetCashProductListItem.ECurrencyType)cp.Currency,
                    IsConsumable = cp.IsConsumable,
                    ItemType = (ResDtoGetCashProductListItem.EItemType)cp.ItemType,
                    Price = cp.Price
                })
                .ToListAsync();

            return rv;
        }
    }
}
