using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Serializable]
public class Serialization<T>
{
	[SerializeField]
	List<T> target;
	public List<T> ToList() { return target; }

	public Serialization(List<T> target)
	{
		this.target = target;
	}
}
public class Util
{
	static public readonly float EPSILON = 0.000001f;

	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
		if (component == null)
            component = go.AddComponent<T>();
        return component;
	}

	public static Component GetOrAddComponent(GameObject go, Type type)
	{
		Component component = go.GetComponent(type);
		if (component == null)
			component = go.AddComponent(type);
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
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
		}
        else
        {
			T findValue = SearchThisTransformWithName<T>(go.transform, name);
			if (findValue != null)
			{
				return findValue;
			}
		}

        return null;
    }

	public static GameObject FindChildWithPath(string path)
	{
		Transform transform = FindChildWithPath<Transform>(path);
		if (transform == null)
			return null;

		return transform.gameObject;
	}

	public static T FindChildWithPath<T>(string path) where T : UnityEngine.Component
	{
		if (path == String.Empty)
			return null;

		string[] paths = path.Split('/');

		Transform findValue = GameObject.Find(paths[0])?.transform;

		if (findValue == null)
		{
			return null;
		}

		for(int i = 1; i < paths.Length; i++)
		{
			findValue = findValue.transform.Find(paths[i]);

			if (findValue == null)
			{
				return null;
			}
		}

		if (findValue != null)
		{
			return findValue.GetComponent<T>();
		}
		return null;
	}

	private  static T SearchThisTransformWithName<T>(Transform parentTransform, string name) where T : UnityEngine.Object
	{
		for (int i = 0; i < parentTransform.childCount; i++)
		{
			Transform transform = parentTransform.GetChild(i);
			if (string.IsNullOrEmpty(name) || transform.name == name)
			{
				T component = transform.GetComponent<T>();
				if (component != null)
					return component;
			}
			else
			{
				T component = SearchThisTransformWithName<T>(transform, name);
				if (component != null)
					return component;
			}
		}

		return null;
	}


	public static List<T> FindChildList<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		List<T> list = new List<T>();
		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Transform transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					T component = transform.GetComponent<T>();
					list.Add(component);
				}
			}
		}
		else
		{
			SearchThisTransformListWithName<T>(ref list, go.transform, name);
		}
		return list;
	}
	private static void SearchThisTransformListWithName<T>(ref List<T> list_, Transform parentTransform, string name) where T : UnityEngine.Object
	{
		if (list_ == null) return;

		for (int i = 0; i < parentTransform.childCount; i++)
		{
			Transform transform = parentTransform.GetChild(i);
			if (string.IsNullOrEmpty(name) || transform.name == name)
			{
				T component = transform.GetComponent<T>();
				if (component != null)
				{
					list_.Add(component);
				}
			}

			SearchThisTransformListWithName<T>(ref list_, transform, name);
			
		}
	}

	public static string ConverTextFromFileText(string text)
	{
		text = text.Replace("\\n", "\n");
		text = text.Replace("\\t", "\t");
		return text;
	}


	public static void WhoAmI(GameObject context, bool applicationPause = false)
	{
#if UNITY_EDITOR
        if (context != null && context is GameObject)
        {
            Selection.activeObject = context;

			if (applicationPause)
			{
                EditorApplication.isPaused = true;

            }
        }
#endif
    }
}
