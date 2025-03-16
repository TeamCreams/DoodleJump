using System;
using System.Collections.Generic;
using System.Text;

namespace GameApiDto.Dtos
{

    public class ReqDtoAddCashProduct
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public float Price { get; set; }
        //ECurrencyType
        public int Currency { get; set; }
        //EItemType
        public int ItemType { get; set; }
        public int Amount { get; set; }
        public bool IsConsumable { get; set; }
    }

    public class ResDtoAddCashProduct
    {
    }
}
