using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserMissionListElement
    {
        public int UserAccountId { get; set; }
        public int MissionId { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class ReqDtoInsertUserMissionList
    {
        public int UserAccountId { get; set; }

        public List<ReqDtoInsertUserMissionListElement> List { get; set; }
    }

    public class ResDtoInsertUserMissionList
    {
        public List<ResDtoInsertUserMissionListElement> List { get; set; }

    }
    public class ResDtoInsertUserMissionListElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
}
