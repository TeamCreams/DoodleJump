using System;
using System.Collections.Generic;
using System.Text;

namespace GameApiDto.Dtos
{
    public class ReqInsertUserAccountScore
    {
        public string? UserName { get; set; }
        public int Score { get; set; }

    }

    public class ResInsertUserAccountScore
    {
    }
}
