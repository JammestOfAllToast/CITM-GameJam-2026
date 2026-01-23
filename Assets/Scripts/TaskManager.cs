using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    public PlayerStats stats;
    public struct Step
    {
        public string id;
        public string location;
        public string name;
    }
    public struct Task
    {
        public string name;
        public int importance;
        public int currentStep;
        public Step[] steps;

        public string[] varStartName;
        public float[] varStartMod; //If used on usage/regen speed, it multiplies, if used on total value, it a sum
        public string[] varEndName;
        public float[] varEndMod; // cant be used on speed, because it will permanently change it. Unless its to out it back to default
    }

    public Task[] taskRepo;
    public Task[] activeTasks;

    void Start()
    {
        stats = GetComponent<PlayerStats>();

        taskRepo = new Task[1];
        activeTasks = new Task[taskRepo.Length];



        taskRepo[0].name = "hello";
        taskRepo[0].importance = 1;
        taskRepo[0].currentStep = 0;
        taskRepo[0].steps = new Step[2];
            taskRepo[0].steps[0].id = "step1";
            taskRepo[0].steps[0].location = "Nav";
            taskRepo[0].steps[0].name = "Do the Thing";
            taskRepo[0].steps[1].id = "step2";
            taskRepo[0].steps[1].location = "Garden";
            taskRepo[0].steps[1].name = "Do the Other Thing";
        taskRepo[0].varStartName = new string[1];
        taskRepo[0].varStartMod = new float[1];
            taskRepo[0].varStartName[0] = "o2rs";
            taskRepo[0].varStartMod[0] = 0.5f;
        taskRepo[0].varEndName = new string[2];
        taskRepo[0].varEndMod = new float[2];
            taskRepo[0].varEndName[0] = "o2rs";
            taskRepo[0].varEndMod[0] = 2f;
            taskRepo[0].varEndName[1] = "o2";
            taskRepo[0].varEndMod[1] = 20;

        TaskAdd(0);
    }

    // Update is called once per frame
    void Update()
    {
        

        //for each task in Active tasks, apply passive effect

        //Every n secs there is a m percent of chance a task from taskRepo is added to activeTasks
    }

    public void StepComplete(string newId)
    {
        if (activeTasks[0].name != default(Task).name) {
            for (int i = 0; i < activeTasks.Length; i++)
            {
                if (activeTasks[i].steps[activeTasks[i].currentStep].id == newId && activeTasks[i].currentStep < activeTasks[i].steps.Length)
                {
                    activeTasks[i].currentStep++;
                    Debug.Log(activeTasks[0].name + " [" + activeTasks[0].currentStep + " / " + activeTasks[0].steps.Length + "]");
            Debug.Log(activeTasks[0].steps[0].name + " [" + activeTasks[0].steps[0].location + "]");
                }

                if (activeTasks[i].currentStep >= activeTasks[i].steps.Length)
                {
                    TaskComplete(i);
                }
            }
        }
    }

    public void TaskComplete(int index)
    {
        Debug.Log("Task finished!");
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
        activeTasks[index] = default(Task);
    }

    public void TaskAdd(int index)
    {
        activeTasks[index] = taskRepo[index];
        Debug.Log(activeTasks[index].name + " [" + activeTasks[index].currentStep + " / " + activeTasks[index].steps.Length + "]");
        Debug.Log(activeTasks[index].steps[activeTasks[0].currentStep].name + " [" + activeTasks[index].steps[activeTasks[index].currentStep].location + "]");


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


