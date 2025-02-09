using System;
using System.Collections.Generic;
using System.Text;


namespace GameApi.Dtos
{
    public class ReqDtoGetOrUpdateUserMissionListElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
    public class ReqDtoGetOrUpdateUserMissionList
    {
        public int UserAccountId { get; set; }

        public List<ReqDtoGetOrUpdateUserMissionListElement> List { get; set; }
    }

    public class ResDtoGetOrUpdateUserMissionList
    {
        public List<ResDtoGetOrUpdateUserMissionListElement> List { get; set; }
    }

    public class ResDtoGetOrUpdateUserMissionListElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
}
