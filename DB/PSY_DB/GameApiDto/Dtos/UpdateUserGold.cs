using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoUpdateUserGold
    {
        public int UserAccountId { get; set; }
        public int Gold { get; set; }
    }

    public class ResDtoUpdateUserGold
    {
        public int Gold { get; set; }
    }
}
