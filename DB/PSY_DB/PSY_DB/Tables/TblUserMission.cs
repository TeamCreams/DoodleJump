﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY_DB.Tables;

public enum EMissionStatus
{
    None,
    Progress,
    Complete,
}

[Comment("UserScore 정보")]
[Table("TblUserMission")]
public partial class TblUserMission
{
    public int Id { get; set; }
    [Comment("TblUserAccount FK")]
    public int UserAccountId { get; set; }

    public int MissionId { get; set; }

    public EMissionStatus MissionStatus { get; set; }

    public int Param1 { get; set; }

    [ForeignKey("UserAccountId")]
    public virtual TblUserAccount? TblUserAccountKeyNavigation { get; set; }
}

// 1 : N
// 유저 : 미션들
// 미션 
// Id, 유저Id, 미션Id, Status, param1, param2

// A몬스터 500마리처치 + 재료 300개 수집