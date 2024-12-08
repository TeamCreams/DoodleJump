using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserStyle
    {
        public int UserAccountId { get; set; }
        // 디자인
        public List<int> CharacterStyle { get; set; } = new List<int>();
        // 업데이트 스택
        public int Evolution { get; set; }
    }

    public class ResDtoInsertUserStyle
    {

    }
}
