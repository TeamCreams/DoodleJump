using System;
using System.Collections.Generic;
using System.Text;


namespace GameApi.Dtos
{
    public class ReqDtoUpdateUserMissionListElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
    public class ReqDtoUpdateUserMissionList
    {
        public int UserAccountId { get; set; }

        public List<ReqDtoUpdateUserMissionListElement> List { get; set; }
    }

    public class ResDtoUpdateUserMissionList
    {
        public List<ResDtoUpdateUserMissionListElement> List { get; set; }
    }

    public class ResDtoUpdateUserMissionListElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
}
