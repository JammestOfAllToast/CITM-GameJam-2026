using DefaultNamespace;
using System.Collections;
using UnityEngine;

public class StepManager : MonoBehaviour, IInteractable
{
    //meant to affect stats, will figue that out later
    public string InteractMessage => objectInteractMessage;

    public string objectInteractMessage;
    public string stepId;
    public bool destroyTool;

    public string requirement;
    public float time = 0;
    public string ogMSG;
        public bool hidden;

    public float waitTime;
    private bool timerStarted = false;

    private float ogWT;
    private float ogT;
    private bool hasSounded;

    private int taskIndex;

    private TaskManager taskManager;
    private AudioSource audioSource;
    public AudioClip sound;

    
    void Start()
    {
        ogWT = waitTime;
        ogT = time;
        taskManager = GameObject.FindWithTag("Player").GetComponent<TaskManager>();
        audioSource = GetComponent<AudioSource>();
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
        if (taskIndex == -1)
        {
            Debug.Log("I'm still here!");
            for(int i = 0; i < taskManager.hullSteps.Length; i++)
            {
                if (taskManager.hullSteps[i].id == stepId)
                {
                    taskIndex = taskManager.FindTaskOfName(taskManager.taskRepo, "Breach In Hull!");
                } 
            }
            for(int i = 0; i < taskManager.fireSteps.Length; i++)
            {
                if (taskManager.fireSteps[i].id == stepId)
                {
                    taskIndex = taskManager.FindTaskOfName(taskManager.taskRepo, "Extinguish Fire");
                } 
            }
        }
        if (taskManager.taskRepo != null && taskIndex != -1) {
            if (taskManager.activeTasks[taskIndex].name == default(TaskManager.Task).name || taskManager.activeTasks[taskIndex].steps[taskManager.activeTasks[taskIndex].currentStep].id != stepId) 
            {
                Hide();
            } else
            {
                if (hidden)
                {
                    Show();
                }
            }
        }
        if (timerStarted)
        {
            if (waitTime <= 0) {
                taskManager.StepComplete(stepId); 
                objectInteractMessage = "Step Complete";
                if (audioSource != null && !hasSounded)
                {
                    audioSource.PlayOneShot(sound, 1f);
                    hasSounded = true;
                }
                if (destroyTool && GameObject.Find(requirement) != null)
                {
                    GameObject.Find(requirement).SetActive(false);
                }
            }
            if (waitTime > -1) {waitTime -= Time.deltaTime;}
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
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }  

        objectInteractMessage = string.Empty;
        hidden = true;
    }

    public void Show()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = true;
        }    

        objectInteractMessage = ogMSG;
        hidden = false;
        waitTime = ogWT;
        time = ogT;
        hasSounded = false;
        timerStarted = false;
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
