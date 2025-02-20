using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoGameStart
    {
        public int UserAccountId { get; set; }
    }
    public class ResDtoGameStart
    {
        public int Energy { get; set; }
        public DateTime LatelyEnergy { get; set; }
    }
}
