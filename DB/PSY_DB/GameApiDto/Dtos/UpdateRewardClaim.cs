using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoUpdateRewardClaim
    {
        public int UserAccountId { get; set; }
        public int Gold { get; set; }
    }

    public class ResDtoUpdateRewardClaim
    {
        public int Gold { get; set; }
        public DateTime LastRewardClaimTime { get; set; }
    }
}
