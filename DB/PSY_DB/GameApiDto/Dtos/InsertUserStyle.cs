using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoUpdateUserStyle
    {
        public int UserAccountId { get; set; }
        public int CharacterId { get; set; }
        // 디자인
        public string? HairStyle { get; set; }
        public string? EyebrowStyle { get; set; }
        public string? EyesStyle { get; set; }
        // 업데이트 스택
        public int Evolution { get; set; }
        public int EvolutionSetLevel { get; set; }
    }

    public class ResDtoUpdateUserStyle
    {

    }
}
