using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowWhenAttribute : PropertyAttribute
{
	public readonly string conditionFieldName;

	public ShowWhenAttribute(string conditionFieldName)
	{
		this.conditionFieldName = conditionFieldName;
	}
}
