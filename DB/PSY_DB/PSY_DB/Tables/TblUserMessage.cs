using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY_DB.Tables;

[Comment("Message 정보")]
[Table("TblUserMessage")]
public partial class TblUserMessage
{
    public int Id { get; set; }
    [Comment("TblUserAccount FK")]
    public int UserAccountId { get; set; }
    public string Message { get; set; }
    public DateTime MessageSentTime { get; set; }
    [Comment("귓속말 시 사용할 메세지 수신 유저 아이디")]
    public int ReceiverUserId { get; set; }

    [ForeignKey("UserAccountId")]
    public virtual TblUserAccount? TblUserAccountKeyNavigation { get; set; }
}