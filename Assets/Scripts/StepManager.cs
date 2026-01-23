using DefaultNamespace;
using UnityEngine;

public class StepManager : MonoBehaviour, IInteractable
{
    //meant to affect stats, will figue that out later
    public string InteractMessage => objectInteractMessage;

    public string objectInteractMessage;
    public string stepId;

    public string requirement;
    public float time = 0;
    public string ogMSG;
    void Start()
    {
        ogMSG = objectInteractMessage;
    }
    public void Interact()
    {
        if (requirement != null)
        {
            if (GameObject.Find(requirement) != null)
            {
                time -= Time.deltaTime;
                objectInteractMessage = ogMSG + " (" + time.ToString("F2") + " seconds left)";
                if (time <= 0) 
                { 
                    GameObject.FindWithTag("Player").GetComponent<TaskManager>().StepComplete(stepId); 
                    objectInteractMessage = "Step Complete";
                }
            } else
            {
                objectInteractMessage = ogMSG + " (Needs " + requirement +")";
            }
        } else
        {
            time -= Time.deltaTime;
            objectInteractMessage = ogMSG + " (" + time.ToString("F2") + " seconds left)";
            if (time <= 0) 
            {
                GameObject.FindWithTag("Player").GetComponent<TaskManager>().StepComplete(stepId); 
                objectInteractMessage = "Step Complete";            
            }
                
        }           
        
    }
}
