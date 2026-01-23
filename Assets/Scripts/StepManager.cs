using DefaultNamespace;
using UnityEngine;

public class StepManager : MonoBehaviour, IInteractable
{
    //meant to affect stats, will figue that out later
    public string InteractMessage => objectInteractMessage;

    public string objectInteractMessage;
    public string stepId;

    public void Interact()
    {
        GameObject.FindWithTag("Player").GetComponent<TaskManager>().StepComplete(stepId);
    }
}
