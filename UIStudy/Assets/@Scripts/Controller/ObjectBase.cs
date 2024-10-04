using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectBase : InitBase
{
	protected int _templateId = -1;
	public int TemplateId
	{
		get => _templateId;
		protected set
		{
			_templateId = value;
			OnChangedTemplateId?.Invoke(value);
		}
	}
	public Action<int> OnChangedTemplateId;
	public Action<Collider> OnTriggerEnter_Event;
	public Action<Collider> OnTriggerExit_Event;


    public override bool Init()
	{
		if(base.Init() == false)
		{
			return false;
		}

		return true;
	}

	public virtual void SetInfo(int templateId)
	{
		// excel 에서 받는 Id => templateId
		// 게임에서 생성된 Id => InstanceId
		// DB에서 저장되는 Id => DbId, ServerId
		TemplateId = templateId;
	}


	public virtual void OnTriggerEnter(Collider collision)
	{
		OnTriggerEnter_Event?.Invoke(collision);
	}

    public virtual void OnTriggerExit(Collider collision)
    {
        OnTriggerExit_Event?.Invoke(collision);
    }

}
