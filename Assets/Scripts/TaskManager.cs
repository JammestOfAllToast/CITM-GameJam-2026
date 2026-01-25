using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    public PlayerStats stats;

    [System.Serializable]
    public struct Step
    {
        public string id;
        public string location;
        public string name;
        //public GameObject sceneObject;
    }

    [System.Serializable]
    public struct Task
    {
        public string name;
        public int importance;
        public int currentStep;
        public Step[] steps;

        [Header("Stat Modifiers")]
        public string[] varStartName;
        public float[] varStartMod; //If used on usage/regen speed, it multiplies, if used on total value, it a sum
        public string[] varEndName;
        public float[] varEndMod; // cant be used on speed, because it will permanently change it. Unless its to out it back to default
    }

    //In the inspector
    public Task[] taskRepo;
    public Step[] hullSteps;
    public Step[] fireSteps;

    // Don't touch in Inspector
    public Task[] activeTasks;

    void Start()
    {
        stats = GetComponent<PlayerStats>();

        if (taskRepo.Length > 0)
        {
            activeTasks = new Task[taskRepo.Length];
        }

        // foreach (Task t in taskRepo)
        // {
        //         foreach (Step s in t.steps)
        //         {
        //             if (s.sceneObject != null)
        //             {
        //                 s.sceneObject.SetActive(false);
        //             }
        //         }
        // }

        // foreach(Step s in hullSteps)
        // {
        //     s.sceneObject.SetActive(false);
        // }

        // foreach(Step s in fireSteps)
        // {
        //     s.sceneObject.SetActive(false);
        // }

        ChooseRandomTask();
        ChooseRandomTask();
        ChooseRandomTask();
    }

    // Update is called once per frame
    void Update()
    {
        

        //for each task in Active tasks, apply passive effect

        //Every n secs there is a m percent of chance a task from taskRepo is added to activeTasks
    }

    public void StepComplete(string newId)
    {
        for (int i = 0; i < activeTasks.Length; i++)
        {
            if (activeTasks[i].name != default(Task).name) {
                if (activeTasks[i].steps[activeTasks[i].currentStep].id == newId && activeTasks[i].currentStep < activeTasks[i].steps.Length)
                {
                    
                    Debug.Log(activeTasks[i].name + " [" + (1 + activeTasks[i].currentStep) + " / " + activeTasks[i].steps.Length + "]");
                    Debug.Log(activeTasks[i].steps[activeTasks[i].currentStep].name + " [" + activeTasks[i].steps[activeTasks[i].currentStep].location + "]");
                    // activeTasks[i].steps[activeTasks[i].currentStep].sceneObject.SetActive(false);
                    activeTasks[i].currentStep++;
                }

                if (activeTasks[i].currentStep >= activeTasks[i].steps.Length)
                {
                    TaskComplete(i);
                }
                else
                {
                    // activeTasks[i].steps[activeTasks[i].currentStep].sceneObject.SetActive(true);
                }
            }
        }
    }

    public void TaskComplete(int index)
    {
        Debug.Log("Task finished!");
        if (activeTasks[index].varEndName != null) 
        {
            for (int i = 0; i < activeTasks[index].varEndName.Length; i++)
            {
                switch(activeTasks[index].varEndName[i])
                {
                    case "o2":
                        stats.Oxygen += activeTasks[index].varEndMod[i];
                        Debug.Log("O2 increased by " + activeTasks[index].varEndMod[i]);
                        break;
                    case "o2us":
                        stats.OxygenUsageSpeed *= activeTasks[index].varEndMod[i];
                        break;
                    case "o2rs":
                        stats.OxygenRegenSpeed *= activeTasks[index].varEndMod[i];
                        Debug.Log("O2 Regen speed increased by " + activeTasks[index].varEndMod[i]);
                        break;
                    case "par":
                        stats.Paranoia += activeTasks[index].varEndMod[i];
                        break;
                    case "parus":
                        stats.ParanoiaUsageSpeed *= activeTasks[index].varEndMod[i];
                        break;
                    case "parrs":
                        stats.ParanoiaRegenSpeed *= activeTasks[index].varEndMod[i];
                        break;
                        //NEEDS TO BE INCREASED AS STATS ARE ADDED!
                }
            }
        }
        activeTasks[index] = default(Task);
    }

    public void ChooseRandomTask()
    {
        int randomTask = Random.Range(0, taskRepo.Length);
        if (taskRepo[randomTask].name == "Breach In Hull!")
        {
            taskRepo[randomTask].steps[0] = hullSteps[Random.Range(0, hullSteps.Length)]; //Add cses for Fire and any other that has random placement
        }
        else if (taskRepo[randomTask].name == "Extinguish Fire")
        {
            taskRepo[randomTask].steps[0] = fireSteps[Random.Range(0, fireSteps.Length)];
        }
        if (activeTasks[randomTask].name == taskRepo[randomTask].name)
        {
            ChooseRandomTask();
        } else
        {
            TaskAdd(randomTask);
        }
    }

    public void TaskAdd(int index)
    {
        activeTasks[index] = taskRepo[index];
        Debug.Log(activeTasks[index].name + " [" + activeTasks[index].currentStep + " / " + activeTasks[index].steps.Length + "]");
        Debug.Log(activeTasks[index].steps[activeTasks[index].currentStep].name + " [" + activeTasks[index].steps[activeTasks[index].currentStep].location + "]");

        // int current = activeTasks[index].currentStep;
        // if (activeTasks[index].steps[current].sceneObject != null)
        // {
        // activeTasks[index].steps[current].sceneObject.SetActive(true);
        // }

        if (activeTasks[index].varStartName != null) 
        {
            for (int i = 0; i < activeTasks[index].varStartName.Length; i++)
            {
                switch(activeTasks[index].varStartName[i])
                {
                    case "o2":
                        stats.Oxygen += activeTasks[index].varStartMod[i];
                        break;
                    case "o2us":
                        stats.OxygenUsageSpeed *= activeTasks[index].varStartMod[i];
                        break;
                    case "o2rs":
                        stats.OxygenRegenSpeed *= activeTasks[index].varStartMod[i];
                        Debug.Log("O2 Regen speed increased by " + activeTasks[index].varStartMod[i]);
                        break;
                    case "par":
                        stats.Paranoia += activeTasks[index].varStartMod[i];
                        break;
                    case "parus":
                        stats.ParanoiaUsageSpeed *= activeTasks[index].varStartMod[i];
                        break;
                    case "parrs":
                        stats.ParanoiaRegenSpeed *= activeTasks[index].varStartMod[i];
                        break;
                }
            }
        } 
    }

}


