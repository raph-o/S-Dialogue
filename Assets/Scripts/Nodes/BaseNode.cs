using XNode;

[CreateNodeMenu("")]
public class BaseNode : Node
{
	public override object GetValue(NodePort port)
	{
		return null;
	}

	public virtual string GetString()
	{
		return string.Empty;
	}
}