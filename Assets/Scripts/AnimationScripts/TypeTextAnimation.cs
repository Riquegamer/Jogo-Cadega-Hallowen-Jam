using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeTextAnimation : MonoBehaviour
{
    public Action typingFinished;

    [SerializeField]private float typeDelay = 0.05f;
     
    [SerializeField]private TextMeshProUGUI textObject;

    public string fullText;

    private Coroutine typingCoroutine;

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    public void Skip()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        textObject.maxVisibleCharacters = fullText.Length;
        typingFinished?.Invoke();
    }



    IEnumerator TypeText()
    {
        textObject.text = fullText;
        textObject.maxVisibleCharacters = 0;
        for (int i = 0; i <= textObject.text.Length; i++)
        {
            textObject.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeDelay);
        }

        typingFinished?.Invoke();
    }

    
}
