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
    public int PlayTime { get; set; }
    public int Scoreboard { get; set; }
    public int Gold { get; set; }
    public int AccumulatedStone { get; set; }
    public int StageLevel { get; set; }

    public DateTime RegisterDate { get; set; }


    [ForeignKey("UserAccountId")]
    public virtual TblUserAccount? TblUserAccountKeyNavigation { get; set; }
}
