using System;
using UnityEngine;

[Serializable]
public struct Choice
{
	[TextArea] public string text;
	public BaseNode exit;
}

public class ChoiceNode : BaseNode
{
	[Input] public BaseNode entry;
	public string speaker;
	[TextArea] public string text;
	[Output(dynamicPortList = true)] public Choice[] choices;

	public override string GetString()
	{
		string data = $"Choice;;{speaker};;{text}";
		foreach (var choice in choices)
		{
			data += $";;{choice.text}";
		}
		return data;
	}

	public string GetPortName(int index)
	{
		return $"choices {index}";
	}
}