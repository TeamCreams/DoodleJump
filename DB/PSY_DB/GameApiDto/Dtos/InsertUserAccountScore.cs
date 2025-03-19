using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserAccountScore
    {
        public int UserAccountId { get; set; }
        public int Score { get; set; }
        public int Time {  get; set; }
        public int AccumulatedStone { get; set; }
        public int StageLevel { get; set; }
        public int Gold { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class ResDtoInsertUserAccountScore
    {
    }
}
