using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY_DB.Tables;

[Comment("User 계정 정보")]
[Table("TblUserAccount")]
public partial class TblUserAccount
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Nickname { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    [SqlDefaultValue("0")]
    public int Gold { get; set; }

    [Comment("캐릭터 아이디")]
    [SqlDefaultValue("20001")]
    public int CharacterId { get; set; }

    [Comment("디자인")]
    [SqlDefaultValue("Afro")]
    public string? HairStyle { get; set; }
    [SqlDefaultValue("AnnoyedEyebrows")]
    public string? EyebrowStyle { get; set; }
    [SqlDefaultValue("Annoyed")]
    public string? EyesStyle { get; set; }
    [Comment("업데이트 스택")]
    [SqlDefaultValue("0")]
    public int Evolution { get; set; }
    [InverseProperty("TblUserAccountKeyNavigation")]
    public virtual ICollection<TblUserScore> TblUserScores { get; set; } = new List<TblUserScore>();
    [InverseProperty("TblUserAccountKeyNavigation")]
    public virtual ICollection<TblUserMission> TblUserMissions { get; set; } = new List<TblUserMission>();
}
