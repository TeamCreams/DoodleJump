using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserAccountScore
    {
        public int UserAccountId { get; set; }
        public int Score { get; set; }
        public int Gold { get; set; }
    }

    public class ResDtoInsertUserAccountScore
    {
    }
}
