using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertEnergy
    {
        public int UserAccountId { get; set; }
        public int Energy { get; set; }
    }

    public class ResDtoInsertEnergy
    {
        public int Energy { get; set; }
        public int PurchaseEnergyCountToday { get; set; }

    }
}
