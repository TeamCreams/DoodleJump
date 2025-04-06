using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class CreatureBase : ObjectBase
{
    protected Stats _stats = null;
    public Stats Stats => _stats;



    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        //데이터 세팅
    }
}
