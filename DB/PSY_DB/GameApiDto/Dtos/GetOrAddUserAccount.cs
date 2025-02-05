using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoGetOrAddUserAccount
    {
        public string? UserName { get; set; }

    }

    public class ResDtoGetOrAddUserAccount
    {
        public string? UserName { get; set; }
        public string? Nickname { get; set; }
        public int UserAccountId { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int HighScore { get; set; }
        public int LatelyScore { get; set; }
        public int Gold {  get; set; }
        public int PlayTime { get; set; }
        public int CharacterId { get; set; }
        public int AccumulatedStone { get; set; }
        // 디자인
        public string? HairStyle { get; set; }
        public string? EyebrowStyle { get; set; }
        public string? EyesStyle { get; set; }
        // 업데이트 스택
        public int Evolution { get; set; }
    }
}
