using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertMissionCompensation
    {
        public int UserAccountId { get; set; }
        public int MissionId { get; set; }
        public int Gold {  get; set; }
    }

    public class ResDtoInsertMissionCompensation
    {
    }
}
