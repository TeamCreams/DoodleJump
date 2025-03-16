using System;
using System.Collections.Generic;
using System.Text;

namespace GameApiDto.Dtos
{

    public class ReqDtoGetCashProductList
    {
    }

    public class ResDtoGetCashProductList
    {
        public List<ResDtoGetCashProductListItem> List { get; set; } = new();
    }

    public class ResDtoGetCashProductListItem
    {
        public enum ECurrencyType
        {
            KRW,
            USD
        }

        public enum EItemType
        {
            Cash,
            Subscription
        }
        public int Id { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public float Price { get; set; }
        public ECurrencyType Currency { get; set; }
        public EItemType ItemType { get; set; }
        public int Amount { get; set; }
        public bool IsConsumable { get; set; }
    }
}
