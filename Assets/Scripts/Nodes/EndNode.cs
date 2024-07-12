public class EndNode : BaseNode
{
    [Input] public BaseNode entry;
    public bool reset;

    public override string GetString()
    {
        return "End";
    }
}