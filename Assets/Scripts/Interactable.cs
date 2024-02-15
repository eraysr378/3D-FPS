using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents; // to add or remove an interaction event 
    [SerializeField] private string promptMessage;

    public virtual void Interact() {

        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }   
    }
    public string GetPromptMessage()
    {
        return promptMessage;
    }
    public void SetPromptMessage(string promptMessage)
    {
        this.promptMessage = promptMessage; 
    }
}
