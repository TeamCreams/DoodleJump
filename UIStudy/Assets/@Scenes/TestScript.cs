using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private List<int> _itemList = new List<int>();


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"RandomItem : {RandomItem()}");
        }
    }


    private int RandomItem()
    {
        _itemList.Clear();
        float range = UnityEngine.Random.Range(0, 1.0f);

        float min = 1;
        float closeValue = 0;

        foreach (var item in Managers.Data.SuberunkerItemDic)
        {
            float difference = Math.Abs(item.Value.Chance - range);
            if (difference < min)
            {
                min = difference;
                closeValue = item.Value.Chance;
            }
            else if (difference == min)
            {
                if (0 <= item.Value.Chance - range)
                {
                    min = difference;
                    closeValue = item.Value.Chance;
                }
            }
        }

        foreach (var item in Managers.Data.SuberunkerItemDic)
        {
            if (closeValue == item.Value.Chance)
            {
                _itemList.Add(item.Value.Id);
            }
        }

        int randItem = UnityEngine.Random.Range(0, _itemList.Count);

        return _itemList[randItem];
    }
}
