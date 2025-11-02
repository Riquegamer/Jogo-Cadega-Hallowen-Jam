using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private GameObject inventory;

    private void Awake()
    {
       inventory = GameObject.FindWithTag("Inventory");
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartGameplayAudio()
    {
        GameObject audioGameplay = GameObject.FindWithTag("SOUNDTRACK");
        AudioSource audioSource = audioGameplay.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else Debug.LogWarning("[GameController] AudioSource do SOUNDTRACK é null!");
    }

    private void Start()
    {
        // Garante que temos todas as dependências antes de carregar o inventário
        if (DBController.Instance == null)
        {
            Debug.LogError("[GameController] DBController.Instance é null!");
            return;
        }
        
        if (InventorySystem.Instance == null)
        {
            Debug.LogError("[GameController] InventorySystem.Instance é null!");
            return;
        }
        
        if (ItemDatabase.Instance == null)
        {
            Debug.LogError("[GameController] ItemDatabase.Instance é null!");
            return;
        }

        // Aguarda um frame para garantir que todos os Awake() foram chamados
        StartCoroutine(LoadInventoryAfterFrame());

        if (inventory != null)
            inventory.SetActive(false);
    }

    private System.Collections.IEnumerator LoadInventoryAfterFrame()
    {
        yield return null; // Aguarda um frame
        Debug.Log("[GameController] Iniciando carregamento do inventário...");
        DBController.Instance.LoadInventory();
        Debug.Log("[GameController] Carregamento do inventário solicitado.");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void ActiveInventory() 
    {
        Debug.Log("Inventory Toggle");
        if (inventory == null) 
        {
            inventory = GameObject.FindWithTag("Inventory");
        }
        if (inventory != null)
        {
            if (inventory.active == true)
                inventory.SetActive(false);
            else
                inventory.SetActive(true);
        }
    }

    public DialogueController FirstDialogue()
    {
        DialogueController dialogueController = GetComponent<DialogueController>();
        return dialogueController;
    }

}
