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
    private bool ogDestroyTool;
    public string requirement;
    public float time = 0;
    public string ogMSG;
        public bool hidden;

    public float waitTime;
    private bool timerStarted = false;

    private float ogWT;
    private float ogT;
    private bool hasSounded;
    private bool altSounded;
    private bool workSounded;
    private bool wasInteractingLastFrame;

    private int taskIndex;

    private TaskManager taskManager;
    private AudioSource audioSource;
    public AudioClip sound;
    public AudioClip altSound;
    public AudioClip workSound;

    
    void Start()
    {
        ogWT = waitTime;
        ogT = time;
        taskManager = GameObject.FindWithTag("Player").GetComponent<TaskManager>();
        audioSource = GetComponent<AudioSource>();
        ogMSG = objectInteractMessage;
        StartCoroutine(Initialize());
        ogDestroyTool = destroyTool;
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
                if (audioSource != null && sound != null && !hasSounded)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(sound, 1f);
                    hasSounded = true;
                }
                if (destroyTool && GameObject.Find(requirement) != null)
                {
                    GameObject.Find(requirement).SetActive(false);
                    destroyTool = false;
                }
            }
            if (waitTime > -1) {waitTime -= Time.deltaTime;}
            if (!hidden) {objectInteractMessage = "Please wait " + waitTime.ToString("F1") + " seconds";}
            if (!altSounded && sound != null && audioSource != null && altSound != null)
            {
                audioSource.PlayOneShot(altSound, 1f);
                altSounded = true;
            }
        }
        if (!wasInteractingLastFrame && workSounded && !hidden && time > 0 && !timerStarted)
        {
            StopWorkSound();
        }
        wasInteractingLastFrame = false;
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
        StopWorkSound();
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
        altSounded = false;
        workSounded = false;
        timerStarted = false;
        destroyTool = ogDestroyTool;
    }   
    public void Interact()
    {
        wasInteractingLastFrame = true;
        if (!hidden) {
            if (requirement != string.Empty)
            {
                if (GameObject.Find(requirement) != null)
                {
                    if (!workSounded && audioSource != null && workSound != null && time > 0)
                    {
                        audioSource.clip = workSound;
                        audioSource.loop = true;
                        audioSource.Play();
                        workSounded = true;
                    }
                    time -= Time.deltaTime;
                    objectInteractMessage = ogMSG + " (" + time.ToString("F1") + " seconds left)";
                    if (time <= 0) 
                    {   
                        StopWorkSound();                      
                        timerStarted = true;
                    }
                } else
                {
                    StopWorkSound();
                    objectInteractMessage = ogMSG + " (Needs " + requirement +")";
                }
            } else
            {
                    if (!workSounded && audioSource != null && workSound != null && time > 0)
                    {
                        audioSource.clip = workSound;
                        audioSource.loop = true;
                        audioSource.Play();
                        workSounded = true;
                    }
                time -= Time.deltaTime;
                objectInteractMessage = ogMSG + " (" + time.ToString("F2") + " seconds left)";
                if (time <= 0) 
                {     
                    StopWorkSound();
                    timerStarted = true;   
                }
                    
            }  
        }         
        
    }
    private void StopWorkSound()
    {
        if (workSounded && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.Stop();
            workSounded = false;
        }
    }
}
