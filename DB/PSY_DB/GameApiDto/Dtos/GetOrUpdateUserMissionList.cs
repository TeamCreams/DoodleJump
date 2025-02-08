using System;
using System.Collections.Generic;
using System.Text;


namespace GameApi.Dtos
{
    public class ReqDtoGetOrUpdateUserMissionElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
    public class ReqDtoGetOrUpdateUserMissionList
    {
        public int UserAccountId { get; set; }

        public List<ReqDtoGetOrUpdateUserMissionElement> List { get; set; }
    }

    public class ResDtoGetOrUpdateUserMissionList
    {
        public List<ResDtoGetOrUpdateUserMissionElement> List { get; set; }
    }

    public class ResDtoGetOrUpdateUserMissionElement
    {
        public int MissionId { get; set; }
        public int MissionStatus { get; set; }
        public int Param1 { get; set; }
    }
}
