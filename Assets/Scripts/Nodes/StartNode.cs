public class StartNode : BaseNode
{
	[Output] public BaseNode exit;

	public override string GetString()
	{
		return "Start";
	}
}