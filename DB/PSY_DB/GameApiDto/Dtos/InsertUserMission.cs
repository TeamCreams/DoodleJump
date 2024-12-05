using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserMission
    {
        public int UserAccountId { get; set; }
        public int MissionId { get; set; }
    }

    public class ReqDtoInsertUserMissions
    {
        public int UserAccountId { get; set; }

        public List<ReqDtoInsertUserMission> List { get; set; }
    }

    public class ResDtoInsertUserMission
    {
       
    }
}
