using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY_DB.Tables;

[Comment("User Cash Product 정보")]
[Table("TblUserCashProduct")]
public partial class TblUserCashProduct
{
    [Required]
    public int Id { get; set; }

    [Required]
    [Comment("TblCashProduct PK")]
    public int ProductId { get; set; } 

    [Required]
    [Comment("User Account Id")]
    public int UserAccountId { get; set; }

    [Required]
    public int Amount { get; set; }

    [Required]
    [SqlDefaultValue("UTC_TIMESTAMP()")]
    public DateTime RegisterDate { get; set; }
    [Required]
    [SqlDefaultValue("UTC_TIMESTAMP()")]
    public DateTime UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }


    [ForeignKey("UserAccountId")]
    public virtual TblUserAccount? TblUserAccountKeyNavigation { get; set; }

    [ForeignKey("ProductId")]
    public virtual TblCashProduct? TblCashProductKeyNavigation { get; set; }
}