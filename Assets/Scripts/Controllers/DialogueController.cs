using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using System;

public enum State
{
    DISABLED,
    WAITING,
    TYPING
}
public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    DialogueUI dialogueUI;

    private State state;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        typeText = FindFirstObjectByType<TypeTextAnimation>();
        dialogueUI = FindFirstObjectByType<DialogueUI>();

        typeText.typingFinished = OnTypingFinished;
    }
    private void Start()
    {
        state = State.DISABLED;
    }

    private void Update()
    {
        switch (state)
        {
            case State.DISABLED:
                break;
            case State.WAITING:
                Waiting();
                break;
            case State.TYPING:
                Typing();
                break;
        }
    }

    public void Next()
    {
        if (currentText == 0)
        {
            dialogueUI.Enable();
        }

        dialogueUI.SetName(dialogueData.talkScript[currentText].characterName);

        typeText.fullText = dialogueData.talkScript[currentText++].text;

        if (currentText == dialogueData.talkScript.Count)
        {
            finished = true;
        }

        typeText.StartTyping();
        state = State.TYPING;
    }

    void Waiting()
    {
        if (!finished)
        {
            Next();
        }
        else
        {
            dialogueUI.Disable();
            state = State.DISABLED;
            currentText = 0;
            finished = false;
            playerController.FinishedTalk();
        }
    }

    void Typing()
    {
    }

    public void SkipTyping()
    {
        if (state == State.TYPING)
        {
            typeText.Skip();
            OnTypingFinished();
        }
    }

    void OnTypingFinished()
    {
        state = State.WAITING;
    }

    private void ShowChoices()
    {

    }

}

//using UnityEngine;
//using System.Collections;

//public class DialogueController : MonoBehaviour
//{
//    [SerializeField] private DialogueData startingNode; // Primeiro diálogo da árvore
//    public DialogueData StartingNode => startingNode;
//    private DialogueData currentNode;
//    private int currentLineIndex;

//    private DialogueUI dialogueUI;
//    private TypeTextAnimation typeText;
//    private PlayerController playerController;

//    private bool isTyping;
//    private bool dialogueEnded;

//    private void Awake()
//    {
//        dialogueUI = FindFirstObjectByType<DialogueUI>();
//        typeText = FindFirstObjectByType<TypeTextAnimation>();
//        playerController = FindFirstObjectByType<PlayerController>();

//        typeText.typingFinished = OnTypingFinished;
//    }

//    private void Start()
//    {
//        StartDialogue(startingNode);
//    }

//    public void StartDialogue(DialogueData node)
//    {
//        currentNode = node;
//        currentLineIndex = 0;
//        dialogueEnded = false;

//        dialogueUI.Enable();
//        ShowNextLine();
//    }

//    public void ShowNextLine()
//    {
//        if (isTyping) return;

//        // Terminou as falas desse nó
//        if (currentLineIndex >= currentNode.lines.Count)
//        {
//            OnNodeFinished();
//            return;
//        }

//        var line = currentNode.lines[currentLineIndex++];
//        dialogueUI.SetName(line.characterName);
//        typeText.fullText = line.text;

//        typeText.StartTyping();
//        isTyping = true;
//    }

//    public void SkipTyping()
//    {
//        if (isTyping)
//        {
//            typeText.Skip();
//            OnTypingFinished();
//        }
//    }

//    private void OnTypingFinished()
//    {
//        isTyping = false;
//    }

//    private void OnNodeFinished()
//    {
//        // Se há escolhas, exibir
//        if (currentNode.choices != null && currentNode.choices.Count > 0)
//        {
//            dialogueUI.ShowChoices(currentNode.choices, OnChoiceSelected);
//        }
//        else
//        {
//            // Nenhuma escolha = fim
//            dialogueUI.Disable();
//            dialogueEnded = true;
//            playerController.FinishedTalk();
//        }
//    }

//    private void OnChoiceSelected(DialogueData nextNode)
//    {
//        dialogueUI.HideChoices();
//        StartDialogue(nextNode);
//    }

//    public bool IsDialogueActive() => !dialogueEnded;

//}

