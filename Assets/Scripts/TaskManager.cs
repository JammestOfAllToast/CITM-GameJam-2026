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

    public Step[] hullSteps;

    void Start()
    {
        stats = GetComponent<PlayerStats>();

        taskRepo = new Task[3];
        activeTasks = new Task[taskRepo.Length];

        hullSteps = new Step[3];                        //for tasks that can happen in various places, have multiple steps that can be added randomly.
        hullSteps[0].name = "Mend Hull (Needs Welder)";
        hullSteps[0].id = "gardenHull";
        hullSteps[0].location = "Garden";
        hullSteps[1].name = "Mend Hull (Needs Welder)";
        hullSteps[1].id = "engineHull";
        hullSteps[1].location = "Engine Room";
        hullSteps[2].name = "Mend Hull (Needs Welder)";
        hullSteps[2].id = "navHull";
        hullSteps[2].location = "Navigation Room";


        taskRepo[0].name = "Broken Radio";
        taskRepo[0].importance = 0;
        taskRepo[0].currentStep = 0;
        taskRepo[0].steps = new Step[1];
            taskRepo[0].steps[0].id = "radio";
            taskRepo[0].steps[0].location = "Outside";
            taskRepo[0].steps[0].name = "Fix antenna (Needs Welder)";

        taskRepo[1].name = "Breach In Hull!";
        taskRepo[1].importance = 2;
        taskRepo[1].currentStep = 0;
        taskRepo[1].steps = new Step[1];
        taskRepo[1].varStartName = new string[1];
        taskRepo[1].varStartMod = new float[1];
            taskRepo[1].varStartName[0] = "o2rs"; //change to ship oxigen usage instead
            taskRepo[1].varStartMod[0] = 0.5f;
        taskRepo[1].varEndName = new string[1];
        taskRepo[1].varEndMod = new float[1];
            taskRepo[1].varEndName[0] = "o2rs";
            taskRepo[1].varEndMod[0] = 2f;

        taskRepo[2].name = "Water Plants";
        taskRepo[2].importance = 1;
        taskRepo[2].currentStep = 0;
        taskRepo[2].steps = new Step[3]; //create a hull breach repo, when this task is added, a random one of the breach in hull steps can be added instead
            taskRepo[2].steps[0].id = "plant1";
            taskRepo[2].steps[0].location = "Garden";
            taskRepo[2].steps[0].name = "Water plant1 (Needs Water Can)";
            taskRepo[2].steps[1].id = "plant2";
            taskRepo[2].steps[1].location = "Garden";
            taskRepo[2].steps[1].name = "Water plant2 (Needs Water Can)";
            taskRepo[2].steps[2].id = "plant3";
            taskRepo[2].steps[2].location = "Garden";
            taskRepo[2].steps[2].name = "Water plant3 (Needs Water Can)";
        taskRepo[2].varStartName = new string[1];
        taskRepo[2].varStartMod = new float[1];
            taskRepo[2].varStartName[0] = "o2rs"; //change to ship oxigen regen instead
            taskRepo[2].varStartMod[0] = 0.5f;
        taskRepo[2].varEndName = new string[1];
        taskRepo[2].varEndMod = new float[1];
            taskRepo[2].varEndName[0] = "o2rs";
            taskRepo[2].varEndMod[0] = 2f;


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
                    activeTasks[i].currentStep++;
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


