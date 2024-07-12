using UnityEngine;

public class ChatNode : BaseNode
{
	[Input] public BaseNode entry;
	public string speaker;
	[TextArea] public string text;
	[Output] public BaseNode exit;

	public override string GetString()
	{
		return $"Chat;;{speaker};;{text}";
	}
}