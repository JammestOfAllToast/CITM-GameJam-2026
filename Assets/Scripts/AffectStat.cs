using DefaultNamespace;
using UnityEngine;

public class AffectStat : MonoBehaviour, IInteractable
{
    //meant to affect stats, will figue that out later
    public string InteractMessage => objectInteractMessage;

    public string objectInteractMessage;

    public void Interact()
    {
        Debug.Log("Interacted");
    }
}
