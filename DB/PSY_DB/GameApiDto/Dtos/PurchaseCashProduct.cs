using System;
using System.Collections.Generic;
using System.Text;

namespace GameApiDto.Dtos
{
    public class ReqDtoPurchaseCashProduct
    {
        public int UserAccountId { get; set; }
        public int CashProductId { get; set; }
        public int Amount { get; set; }
    }

    public class ResDtoPurchaseCashProduct
    {
    }
}
