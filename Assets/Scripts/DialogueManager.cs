using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    [Header("Dialogue")]
    [SerializeField] private List<DialogueGraph> dialogues;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject choicePanel;
    
    [Header("Text")]
    [SerializeField] private TMP_Text speaker;
    [SerializeField] private TMP_Text dialogue;

    private Coroutine _dialogueParser;
    private List<Button> _choiceButtons = new();
    
    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        foreach (Transform choiceTransform in choicePanel.transform)
            _choiceButtons.Add(choiceTransform.GetComponent<Button>());
    }

    public DialogueGraph GetDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Count) throw new ArgumentException("Index out of range.");
        return dialogues[index];
    }

    public void ShowDialogue(DialogueGraph dialogueGraph)
    {
        if (!dialoguePanel.activeSelf) dialoguePanel.SetActive(true);
        speaker.text = string.Empty;
        dialogue.text = string.Empty;
        _dialogueParser = StartCoroutine(ParseDialogue(dialogueGraph));
    }

    private IEnumerator ParseDialogue(DialogueGraph dialogueGraph)
    {
        string data = dialogueGraph.currentNode.GetString();
        string[] dataParts = data.Split(";;");
        
        if (dataParts[0] == "Start") NextDialogue(dialogueGraph, "exit");
        
        else if (dataParts[0] == "End")
            yield return StartCoroutine(ParseEnd(dialogueGraph));
        
        else if (dataParts[0] == "Choice")
            yield return StartCoroutine(ParseChoice(dialogueGraph, dataParts));
        
        else if (dataParts[0] == "Chat")
        {
            yield return StartCoroutine(ParseChat(dataParts));
            NextDialogue(dialogueGraph, "exit");
        }
    }

    private IEnumerator ParseEnd(DialogueGraph dialogueGraph)
    {
        if (dialogueGraph.currentNode is EndNode { reset: true })
            dialogueGraph.Reset();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        dialoguePanel.SetActive(false);
    }

    private IEnumerator ParseChoice(DialogueGraph dialogueGraph, string[] dataParts)
    {
        choicePanel.SetActive(true);
        string speakerTxt = dataParts[1];
        speaker.text = speakerTxt;

        string text = dataParts[2];
        yield return StartCoroutine(AnimateText(text, dialogue));
        
        for (int i = 3; i < dataParts.Length; i++)
        {
            int choiceIndex = i - 3;
            Button choiceBtn = _choiceButtons[choiceIndex];
            choiceBtn.onClick.AddListener(() => OnChoiceSelected(choiceIndex, dialogueGraph));
            
            TMP_Text choiceTxt = choiceBtn.GetComponentInChildren<TMP_Text>();
            choiceTxt.text = dataParts[i];
        }

        yield return null;
    }
    
    private void OnChoiceSelected(int index, DialogueGraph dialogueGraph)
    {
        ChoiceNode choiceNode = dialogueGraph.currentNode as ChoiceNode;
        string portName = choiceNode.GetPortName(index);
        choicePanel.SetActive(false);
        NextDialogue(dialogueGraph, portName);
    }

    private IEnumerator ParseChat(string[] dataParts)
    {
        string speakerTxt = dataParts[1];
        string text = dataParts[2];
        speaker.text = speakerTxt;
        
        yield return StartCoroutine(AnimateText(text, dialogue));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }
    
    private IEnumerator AnimateText(string text, TMP_Text tmpText)
    {
        tmpText.text = string.Empty;
        int i = 0;
        while (i < text.Length)
        {
            tmpText.text += text[i];
            i++;
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void NextDialogue(DialogueGraph dialogueGraph, string fieldName)
    {
        if (_dialogueParser is not null)
        {
            StopCoroutine(_dialogueParser);
            _dialogueParser = null;
        }
        
        BaseNode nextNode = dialogueGraph.FindNextNode(fieldName);
        dialogueGraph.currentNode = nextNode;
        _dialogueParser = StartCoroutine(ParseDialogue(dialogueGraph));
    }
}