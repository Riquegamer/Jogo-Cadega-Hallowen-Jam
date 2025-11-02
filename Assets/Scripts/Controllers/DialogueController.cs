using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public enum State
{
    DISABLED,
    WAITING,
    TYPING
}
public class DialogueController : MonoBehaviour
{
    public DialogueData dialogueData;
    [SerializeField] private DialogueChoice dialogueChoices;
    [SerializeField] private bool isChoiceDialogue;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private TextMeshProUGUI[] textChoices;
    int currentText = 0;
    bool finished = false;

    [SerializeField]TypeTextAnimation typeText;
    [SerializeField]DialogueUI dialogueUI;

    private AudioSource audioSource;
    private State state;

    private PlayerController playerController;
    [SerializeField]private bool isStartDialogue;
    [HideInInspector]public bool terminated;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        //typeText = FindFirstObjectByType<TypeTextAnimation>();
        //dialogueUI = FindFirstObjectByType<DialogueUI>();

        typeText.typingFinished += OnTypingFinished;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        state = State.DISABLED;

        if (isStartDialogue)
        {
            StartCoroutine(StartInitialDialogue());
            Debug.Log("Passou aqui" + gameObject.name);
        }
    }

    private IEnumerator StartInitialDialogue()
    {
        yield return new WaitForEndOfFrame(); // garante que a cena terminou de carregar
        Next();
        if(audioSource != null)
        {
            audioSource.Play();
        }
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
        else if (isChoiceDialogue && dialogueChoices.Escolhas.Count > 0)
        {
            activeChoice();
        }
        else
        {
            dialogueUI.Disable();
            state = State.DISABLED;
            currentText = 0;
            finished = false;
            playerController.FinishedTalk();
            terminated = true;
            if (audioSource != null)
            {
                audioSource.Stop();
                GameController.instance.StartGameplayAudio();
            }
            
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

    public void RestartDialogue()
    {
        // Reseta os índices e flags
        currentText = 0;
        finished = false;
        state = State.DISABLED;

        // Limpa o texto do UI
        if (dialogueUI != null)
        {
            dialogueUI.Disable();
        }

        // Para qualquer digitação em andamento
        if (typeText != null)
        {
            typeText.Skip();
        }
    }

    void activeChoice()
    {
        for (int i = 0; i < dialogueChoices.Escolhas.Count; i++)
        {
            textChoices[i].text = dialogueChoices.Escolhas[i].optionText;
        }

        choicePanel.SetActive(true);
    }

    public void Choice(int choiceIndex)
    {
        dialogueData = dialogueChoices.Escolhas[choiceIndex].consequencia;
        choicePanel.SetActive(false);
        isChoiceDialogue = false;
        RestartDialogue();
    }

}