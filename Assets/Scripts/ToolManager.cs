using DefaultNamespace;
using UnityEngine;

public class ToolManager : MonoBehaviour, IInteractable
{
    public string InteractMessage => objectInteractMessage;
    public string objectInteractMessage;
    public string toolName;
    
    public ObjectHolder objectHolder;

    void Start()
    {
        objectHolder = GameObject.FindWithTag("ObjectHolder").GetComponent<ObjectHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (objectHolder.holding != null)
        {
            objectHolder.SwitchObj(objectHolder.holding.name, name);
        } else
        {
            objectHolder.TurnOn(toolName);
        }
    }
}
