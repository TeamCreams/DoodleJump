using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserAccountScore
    {
        public string? UserName { get; set; }
        public int Score { get; set; }

    }

    public class ResDtoInsertUserAccountScore
    {
    }
}
