using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    Image background;
    TextMeshProUGUI nameText;
    TextMeshProUGUI dialogueText;

    [SerializeField]private float _speed;
    [SerializeField]private bool open;

    private void Start()
    {
        
    }
    private void Awake()
    {
        background = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dialogueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (open) 
        {
            background.fillAmount = Mathf.Lerp(background.fillAmount, 1f, Time.deltaTime * _speed);
        }
        else
        {
            background.fillAmount = Mathf.Lerp(background.fillAmount, 0f, Time.deltaTime * _speed);
        }
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void Enable()
    {
        background.fillAmount = 0f;
        open = true;
    }

    public void Disable()
    {
        open = false;
        nameText.text = "";
        dialogueText.text = "";
    }

}
