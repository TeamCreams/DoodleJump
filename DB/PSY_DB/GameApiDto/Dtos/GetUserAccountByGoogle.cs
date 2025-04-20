using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoGetUserAccountByGoogle
    {
        public string? GoogleAccount { get; set; }
    }

    public class ResDtoGetUserAccountByGoogle
    {
        public int UserAccountId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Nickname { get; set; }
        public string? GoogleAccount { get; set; }

        public int HighScore { get; set; }
        public int LatelyScore { get; set; }
        public int Gold { get; set; }
        public int PlayTime { get; set; }
        public int CharacterId { get; set; }
        public int AccumulatedStone { get; set; }
        public int StageLevel { get; set; }
        // 디자인
        public string? HairStyle { get; set; }
        public string? EyebrowStyle { get; set; }
        public string? EyesStyle { get; set; }
        // 업데이트 스택
        public int Evolution { get; set; }
        public int EvolutionSetLevel { get; set; }
        // 에너지 관련
        public DateTime LatelyEnergy { get; set; }
        public int Energy { get; set; }
        public int PurchaseEnergyCountToday { get; set; }
        public DateTime LastRewardClaimTime { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
