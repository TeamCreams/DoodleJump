using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoGetUserMissionList
    {
        public int UserAccountId { get; set; }
    }
    public class ResDtoGetUserMissionList
    {
        public List<ResDtoGetUserMissionListElement> List { get; set; }
    }

    public class ResDtoGetUserMissionListElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
}
