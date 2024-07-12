using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "S-Dialog/Dialogue")]
public class DialogueGraph : NodeGraph
{
    public BaseNode startNode; 
    public BaseNode currentNode;

    public void Reset()
    {
        currentNode = startNode;
    }
    
    public BaseNode FindNextNode(string fieldName)
    {
        foreach (var port in currentNode.Ports)
        {
            if (port.fieldName == fieldName)
                return port.Connection.node as BaseNode;
        }

        return null;
    }
}