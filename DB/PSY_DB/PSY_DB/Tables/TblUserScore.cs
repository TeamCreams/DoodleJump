using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY_DB.Tables;

[Comment("UserScore 정보")]
[Table("TblUserScore")]
public partial class TblUserScore
{
    public int Id { get; set; }
    [Comment("TblUserScore FK")]
    public int UserAccountId { get; set; }
    public int History { get; set; }
    public int Gold { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }


    [ForeignKey("UserAccountId")]
    public virtual TblUserAccount? TblUserAccountKeyNavigation { get; set; }
}
