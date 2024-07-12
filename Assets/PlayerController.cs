using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool _canInteract;
    private Dialogue _dialogue;
    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(x, y) * (5 * Time.deltaTime);

        if (_canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
                DialogueManager.Instance.ShowDialogue(_dialogue.DialogueGraph);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Dialogue dialogue))
        {
            _dialogue = dialogue;
            _canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Dialogue dialogue))
        {
            _dialogue = dialogue;
            _canInteract = false;
        }
    }
}
