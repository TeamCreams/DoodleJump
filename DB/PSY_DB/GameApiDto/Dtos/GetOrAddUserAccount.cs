using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoGetOrAddUserAccount
    {
        public string? UserName { get; set; }

    }

    public class ResDtoGetOrAddUserAccount
    {
        public string? UserName { get; set; }
        public string? Nickname { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int HighScore { get; set; }
        public int LatelyScore { get; set; }
    }
}
