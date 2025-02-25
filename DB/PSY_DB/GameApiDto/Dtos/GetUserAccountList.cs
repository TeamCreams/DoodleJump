using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoGetUserAccountList
    {

    }
    public class ResDtoGetUserAccountList
    {
        public List<ResDtoGetUserAccountListElement> List { get; set; }
    }

    public class ResDtoGetUserAccountListElement
    {
        public int UserAccountId { get; set; }
        public string? Nickname { get; set; }
        public int HighScore { get; set; }
    }
}
