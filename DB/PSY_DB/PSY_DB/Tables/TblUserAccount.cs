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
    [Required]
    public int Id { get; set; }
    [Comment("계정 이름")]
    [SqlDefaultValue("NewAccount")]
    public string? UserName { get; set; }
    [Comment("계정 비밀번호")]
    [SqlDefaultValue("0000")]
    public string? Password { get; set; }
    [Required]
    [Comment("계정 닉네임")]
    [SqlDefaultValue("Empty")]
    public string? Nickname { get; set; }
    [Comment("구글 계정")]
    [SqlDefaultValue("0000")]
    public string? GoogleAccount { get; set; }


    //[Required]
    //현재 컬럼상 DeletedDate말고는 모두 Required 처리를 해주는게 맞다.
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
    [SqlDefaultValue("140003")]
    public int Evolution { get; set; }
    [Comment("EvolutionSet Level")]
    public int EvolutionSetLevel { get; set; }
    [Required]
    [Comment("마지막으로 에너지를 얻은 시간")]
    [SqlDefaultValue("UTC_TIMESTAMP()")]
    public DateTime LatelyEnergy { get; set; }
    [Required]
    [SqlDefaultValue("10")]
    public int Energy { get; set; }
    [Comment("하루 중 에너지를 처음으로 구매한 시간")]
    public DateTime FirstPurchaseEnergyTime { get; set; }
    [Comment("에너지를 구매한 횟수")]
    [SqlDefaultValue("0")]
    public int PurchaseEnergyCountToday { get; set; }

    [Comment("접속 보상 시간")]
    public DateTime LastRewardClaimTime { get; set; }

[Required]
    public DateTime RegisterDate { get; set; }
    [Required]
    public DateTime UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    [InverseProperty("TblUserAccountKeyNavigation")]
    public virtual ICollection<TblUserScore> TblUserScores { get; set; } = new List<TblUserScore>();
    [InverseProperty("TblUserAccountKeyNavigation")]
    public virtual ICollection<TblUserMission> TblUserMissions { get; set; } = new List<TblUserMission>();
    [InverseProperty("TblUserAccountKeyNavigation")]
    public virtual ICollection<TblUserMessage> TblUserMessage { get; set; } = new List<TblUserMessage>();
    [InverseProperty("TblUserAccountKeyNavigation")]
    public virtual ICollection<TblUserCashProduct> TblUserCashProduct { get; set; } = new List<TblUserCashProduct>();
}
