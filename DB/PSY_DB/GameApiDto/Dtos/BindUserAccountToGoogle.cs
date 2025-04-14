using System;
using System.Collections.Generic;
using System.Text;

namespace GameApi.Dtos
{
    public class ReqDtoBindUserAccountToGoogle
    {
        public int UserAccountId { get; set; }

        public string? GoogleAccount { get; set; }
    }

    public class ResDtoBindUserAccountToGoogle
    {
    }
}
