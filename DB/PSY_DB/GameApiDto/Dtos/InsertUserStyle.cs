using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserStyle
    {
        public int UserAccountId { get; set; }
        // 디자인
        public int HairStyle { get; set; }
        public int EyebrowStyle { get; set; }
        public int EyesStyle { get; set; }
        // 업데이트 스택
        public int Evolution { get; set; }
    }

    public class ResDtoInsertUserStyle
    {

    }
}
