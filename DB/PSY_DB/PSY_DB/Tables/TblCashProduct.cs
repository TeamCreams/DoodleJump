using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY_DB.Tables;

public enum ECurrencyType
{
    KRW,
    USD
}

public enum EItemType
{
    Cash,
    Subscription
}
[Comment("Cash Product 정보 Update 금지.")]
[Table("TblCashProduct")]
public partial class TblCashProduct
{
    [Required]
    public int Id { get; set; }

    [Required]
    [Comment("Store Product Id")]
    public string ProductId { get; set; } = string.Empty;

    [Required]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public float Price { get; set; }

    [Required]
    [Comment("0 : KRW, 1 : USD")]
    public ECurrencyType Currency { get; set; }

    [Required]
    [Comment("0 : Cash, 1 : Subscription")]
    public EItemType ItemType { get; set; }

    [Required]
    public int Amount { get; set; }

    [Required]
    [Comment("구독 중인지")]
    public bool IsConsumable { get; set; }

    [Required]
    [SqlDefaultValue("UTC_TIMESTAMP()")]
    public DateTime RegisterDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    [InverseProperty("TblCashProductKeyNavigation")]
    public virtual ICollection<TblUserCashProduct> TblUserCashProducts { get; set; } = new List<TblUserCashProduct>();
}