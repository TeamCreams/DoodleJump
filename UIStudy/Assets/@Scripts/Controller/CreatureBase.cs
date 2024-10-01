using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreatureBase : ObjectBase
{
    protected Stats _stats = new Stats(new CreatureInfoData());



    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        //데이터 세팅
    }
}
