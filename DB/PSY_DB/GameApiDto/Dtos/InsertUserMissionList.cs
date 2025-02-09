using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserMissionListElement
    {
        public int UserAccountId { get; set; }
        public int MissionId { get; set; }
    }

    public class ReqDtoInsertUserMissionList
    {
        public int UserAccountId { get; set; }

        public List<ReqDtoInsertUserMissionListElement> List { get; set; }
    }

    public class ResDtoInsertUserMissionList
    {
       
    }
}
