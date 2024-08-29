using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserSystem
{
	private int _life = 3;
	public int Life
	{
		get => _life;
		set
		{
			if(_life != value)
			{
				_life = value;
				OnChangedLife?.Invoke(value);
			}
		}
	}

	public Action<int> OnChangedLife;
}
