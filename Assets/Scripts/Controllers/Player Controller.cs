using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField]private float _moveSpeed;
    private bool canMove = true;
    [SerializeField]private Rigidbody2D _rb;
    private Vector2 direction;

    [Header("Dialogue")]
    private DialogueController dialogueController;
    private bool canTalk = false;
    private bool isTalking = false;
    private GameObject dialogueTutorial;

    #endregion

    #region UnityCallbacks
    private void Awake()
    {
        dialogueTutorial = GameObject.Find("DialogueTutorial");
        dialogueTutorial.SetActive(false);
        // _rb.GetComponent<Rigidbody2D>();
        dialogueController = FindFirstObjectByType<DialogueController>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(direction.x, direction.y, 0f);
        _rb.MovePosition(_rb.position + Vector2.ClampMagnitude(movement, 1f) * _moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            dialogueTutorial.SetActive(true);
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            dialogueTutorial.SetActive(false);
            canTalk = false;
        }
    }

    #endregion

    #region Move
    public void move(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            direction = context.ReadValue<Vector2>();
        }
    }
    #endregion

    #region Talk
    public void talk(InputAction.CallbackContext context)
    {
        if (context.performed && canTalk)
        {
            dialogueController.Next();
            dialogueTutorial.SetActive(false);
            canTalk = false;
            canMove = false;
            isTalking = true;
        }
    }

    public void skipTyping(InputAction.CallbackContext context)
    {
        if (context.performed && isTalking)
        {
            dialogueController.SkipTyping();
        }
    }

    public void FinishedTalk()
    {
        canMove = true;
        canTalk = true;
        isTalking = false;
    }

    #endregion

    #region Inventory
    public void ActiveInventory(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            GameController.instance.ActiveInventory();
        }
    }
    #endregion

}
