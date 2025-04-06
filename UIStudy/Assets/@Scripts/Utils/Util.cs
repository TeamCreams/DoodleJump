using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using UnityEngine;

public static class Util
{
	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
			component = go.AddComponent<T>();

		return component;
	}

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		Transform transform = FindChild<Transform>(go, name, recursive);
		if (transform == null)
			return null;

		return transform.gameObject;
	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Transform transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name.Trim() == name)
				{
					T component = transform.GetComponent<T>();
					if (component != null)
						return component;
				}
			}
		}
		else
		{
			Stack<Transform> s = new Stack<Transform>();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                s.Push(transform);
			}

			while(s.Count != 0)
			{
                Transform current = s.Peek();
				s.Pop();

                if (string.IsNullOrEmpty(name) || current.name == name)
                {
                    T component = current.GetComponent<T>();
                    if (component != null)
                        return component;
                }

                for (int i = 0; i < current.childCount; i++)
                {
                    Transform transform = current.GetChild(i);
                    s.Push(transform);
                }
            }
   //         foreach (T component in go.GetComponentsInChildren<T>())
			//{
			//	if (string.IsNullOrEmpty(name) || component.name == name)
			//		return component;
			//}
		}

		return null;
	}

	public static T ParseEnum<T>(string value)
	{
		return (T)Enum.Parse(typeof(T), value, true);
	}
}



public static class QueryStringParser
{
    public static Dictionary<string, string> ParseQueryString(string query)
    {
        var dict = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(query))
            return dict;

        // 맨 앞의 '?' 제거
        if (query.StartsWith("?"))
            query = query.Substring(1);

        var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var kvp = pair.Split('=', 2);
            var key = WebUtility.UrlDecode(kvp[0]);
            var value = kvp.Length > 1 ? WebUtility.UrlDecode(kvp[1]) : "";
            dict[key] = value;
        }

        return dict;
    }
}
