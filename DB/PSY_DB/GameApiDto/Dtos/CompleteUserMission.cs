using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoCompleteUserMission
    {
        public int UserAccountId { get; set; }
        public int MissionId { get; set; }
        public int Param1 { get; set; }
        public int Gold { get; set; }
    }

    public class ResDtoCompleteUserMission
    {
    }
}
