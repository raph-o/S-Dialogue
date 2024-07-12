using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private int conversationIndex;
    
    private DialogueGraph _dialogueGraph;
    
    public DialogueGraph DialogueGraph => _dialogueGraph;

    private void Start()
    {
        _dialogueGraph = DialogueManager.Instance.GetDialogue(conversationIndex);
    }
}
