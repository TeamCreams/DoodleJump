using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoUpdateEnergy
    {
        public int UserAccountId { get; set; }
    }
    public class ResDtoUpdateEnergy
    {
        public int Energy {  get; set; }
        public DateTime LatelyEnergy { get; set; }
    }
}
