using DefaultNamespace;
using System.Collections;
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

    public bool hidden;

    public float waitTime;
    private bool timerStarted = false;

    private int taskIndex;

    private TaskManager taskManager;
    void Start()
    {
        taskManager = GameObject.FindWithTag("Player").GetComponent<TaskManager>();
        ogMSG = objectInteractMessage;
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        // Wait until TaskManager is ready
        
        while (taskManager == null || taskManager.taskRepo == null)
        {
            yield return null; // Wait one frame
        }
        Hide();
        taskIndex = findMyTask();
    }


    void Update()
    {
        if (taskManager.taskRepo != null && taskIndex != -1) {
            if (taskManager.activeTasks[taskIndex].name == default(TaskManager.Task).name || taskManager.activeTasks[taskIndex].steps[taskManager.activeTasks[taskIndex].currentStep].id != stepId) 
            {
                Hide();
            } else
            {
                Show();
            }
        }
        if (timerStarted)
        {
            if (waitTime <= 0) {
                taskManager.StepComplete(stepId); 
                objectInteractMessage = "Step Complete";
            }
            waitTime -= Time.deltaTime;
            if (!hidden) {objectInteractMessage = "Please wait " + waitTime.ToString("F1") + " seconds";}
        }

    }

    public int findMyTask()
    {
        for (int i = 0; i < taskManager.taskRepo.Length; i++)
        {
            for (int j = 0; j < taskManager.taskRepo[i].steps.Length; j++)
            {
                if (taskManager.taskRepo[i].steps[j].id == stepId)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public void Hide()
    {
        GetComponent<MeshRenderer>().enabled = false;
        objectInteractMessage = string.Empty;
        hidden = true;
    }

    public void Show()
    {
        GetComponent<MeshRenderer>().enabled = true;
        if (objectInteractMessage == string.Empty) { objectInteractMessage = ogMSG; }
        hidden = false;
    }

    public void Interact()
    {
        if (!hidden) {
            if (requirement != string.Empty)
            {
                if (GameObject.Find(requirement) != null)
                {
                    time -= Time.deltaTime;
                    objectInteractMessage = ogMSG + " (" + time.ToString("F1") + " seconds left)";
                    if (time <= 0) 
                    {                         
                        timerStarted = true;
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
                    timerStarted = true;   
                }
                    
            }  
        }         
        
    }
}
