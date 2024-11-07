using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoInsertUserAccountNickname
    {
        public string? UserName { get; set; }

        public string? Nickname { get; set; }
    }

    public class ResDtoInsertUserAccountNickname
    {
    }
}
