using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    public PlayerStats stats;
    public TimeController timeController;
    public PlayerMovement movement;
    public ObjectHolder objectHolder;

    public int minTimeToTask;
    public int maxTimeToTask;

    
    public bool DEWIT;

    public struct Step
    {
        public string id;
        public string location;
        public string name;
        //public GameObject sceneObject;
    }


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
    public Step[] arcadeSteps;
    public Step[] waterSteps;
    

    // Don't touch in Inspector
    public Task[] activeTasks;


    public int countActiveTasks = 0;
    private float taskTimer = 0;

    void Start()
    {
        objectHolder = GameObject.FindWithTag("ObjectHolder").GetComponent<ObjectHolder>();
        stats = GetComponent<PlayerStats>();
        timeController = GameObject.FindWithTag("TimeWizard").GetComponent<TimeController>();
        movement = GetComponent<PlayerMovement>();

        taskRepo = new Task[12];
        activeTasks = new Task[taskRepo.Length];

// BREACH IN HULL!
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

// HULL STEPS
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

// BROKEN RADIO
        taskRepo[0].name = "Broken Radio";
        taskRepo[0].importance = 0;
        taskRepo[0].currentStep = 0;
        taskRepo[0].steps = new Step[1];
            taskRepo[0].steps[0].id = "radio";
            taskRepo[0].steps[0].location = "Outside";
            taskRepo[0].steps[0].name = "Fix antenna (Needs Welder)";

// WATER PLANTS
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

// FIX LIGHTS
        // If lights below 0, then task is triggered
        taskRepo[3].name = "Fix Lights";
        taskRepo[3].importance = 2;
        taskRepo[3].currentStep = 0;
        taskRepo[3].steps = new Step[5];
            taskRepo[3].steps[0].id = "panelOne";
            taskRepo[3].steps[0].location = "Outside";
            taskRepo[3].steps[0].name = "Place Solar Panel (Find the panel)";

            taskRepo[3].steps[1].id = "panelTwo";
            taskRepo[3].steps[1].location = "Outside";
            taskRepo[3].steps[1].name = "Fix Solar Panels (Find the panel)";

            taskRepo[3].steps[2].id = "panelThree";
            taskRepo[3].steps[2].location = "Outside";
            taskRepo[3].steps[2].name = "Fix Solar Panels (Find the panel)";

            taskRepo[3].steps[3].id = "panelFour";
            taskRepo[3].steps[3].location = "Outside";
            taskRepo[3].steps[3].name = "Fix Solar Panels (Find the panel)";

            taskRepo[3].steps[4].id = "lights";
            taskRepo[3].steps[4].location = "Engine Room";
            taskRepo[3].steps[4].name = "Reset breakers (fix solar panels first)";
        taskRepo[2].varStartName = new string[1];
        taskRepo[2].varStartMod = new float[1];
            taskRepo[2].varStartName[0] = "o2rs"; //change to ship oxigen regen instead
            taskRepo[2].varStartMod[0] = 0.5f;
        taskRepo[2].varEndName = new string[1];
        taskRepo[2].varEndMod = new float[1];
            taskRepo[2].varEndName[0] = "o2rs";
            taskRepo[2].varEndMod[0] = 2f; 
       
// EXTINGUISH FIRE
        taskRepo[4].name = "Extinguish Fire";
        taskRepo[4].importance = 2;
        taskRepo[4].currentStep = 0;
        taskRepo[4].steps = new Step[1];
        taskRepo[4].varStartName = new string[1];
        taskRepo[4].varStartMod = new float[1];
            taskRepo[4].varStartName[0] = "o2rs"; //change to ship oxigen regen instead
            taskRepo[4].varStartMod[0] = 0.5f;
        taskRepo[4].varEndName = new string[1];
        taskRepo[4].varEndMod = new float[1];
            taskRepo[4].varEndName[0] = "o2rs";
            taskRepo[4].varEndMod[0] = 2f;

// FIRE STEPS        
        fireSteps = new Step[2];                        //for tasks that can happen in various places, have multiple steps that can be added randomly.
        fireSteps[0].name = "Extinguish fire (Needs extinguisher)";
        fireSteps[0].id = "machineFire";
        fireSteps[0].location = "Machine Room";
        
        fireSteps[1].name = "Extinguish fire (Needs extinguisher)";
        fireSteps[1].id = "cryoFire";
        fireSteps[1].location = "Cryostasis Chamber";

// REFUEL SHIP
        taskRepo[5].name = "Refuel Ship";
        taskRepo[5].importance = 1;
        taskRepo[5].currentStep = 0;
        taskRepo[5].steps = new Step[5];
            taskRepo[5].steps[0].id = "harvestOne";
            taskRepo[5].steps[0].location = "Garden";
            taskRepo[5].steps[0].name = "Harvest Fuel Plant";

            taskRepo[5].steps[1].id = "harvestTwo";
            taskRepo[5].steps[1].location = "Garden";
            taskRepo[5].steps[1].name = "Harvest Fuel Plant";

            taskRepo[5].steps[2].id = "harvestThree";
            taskRepo[5].steps[2].location = "Garden";
            taskRepo[5].steps[2].name = "Harvest Fuel Plant";

            taskRepo[5].steps[3].id = "harvestAlgae";
            taskRepo[5].steps[3].location = "Garden";
            taskRepo[5].steps[3].name = "Pick up algae from basket";

            taskRepo[5].steps[4].id = "makeFuel";
            taskRepo[5].steps[4].location = "Engine Room";
            taskRepo[5].steps[4].name = "Create Fuel";

// REROUTE NAVIGATION
        taskRepo[6].name = "Reroute Navigation";
        taskRepo[6].importance = 1;
        taskRepo[6].currentStep = 0;
        taskRepo[6].steps = new Step[1];
            taskRepo[6].steps[0].id = "pc";
            taskRepo[6].steps[0].location = "Navigation Room";
            taskRepo[6].steps[0].name = "Reroute the navigation towards destination";

// FIX WATER PIPES
        taskRepo[7].name = "Fix Water Pipes";
        taskRepo[7].importance = 1;
        taskRepo[7].currentStep = 0;
        taskRepo[7].steps = new Step[3];

// WATER STEPS
        waterSteps = new Step[5];
        waterSteps[0].id = "pipeOne";
        waterSteps[0].location = "Cryostasis Chamber";
        waterSteps[0].name = "Fix Water Pipe (Needs Wrench)";

        waterSteps[1].id = "pipeTwo";
        waterSteps[1].location = "Cryostasis Chamber";
        waterSteps[1].name = "Fix Water Pipe (Needs Wrench)";

        waterSteps[2].id = "pipeThree";
        waterSteps[2].location = "Cryostasis Chamber";
        waterSteps[2].name = "Fix Water Pipe (Needs Wrench)";

        waterSteps[3].id = "pipeFour";
        waterSteps[3].location = "Cryostasis Chamber";
        waterSteps[3].name = "Fix Water Pipe (Needs Wrench)";

        waterSteps[4].id = "pipeFive";
        waterSteps[4].location = "Cryostasis Chamber";
        waterSteps[4].name = "Fix Water Pipe (Needs Wrench)";

// FIX ARCADES
        taskRepo[8].name = "Fix Arcade";
        taskRepo[8].importance = 1;
        taskRepo[8].currentStep = 0;
        taskRepo[8].steps = new Step[1];

// ARCADE STEPS
        arcadeSteps = new Step[3];
        arcadeSteps[0].name = "Fix Arcade (Needs Wrench) ";
        arcadeSteps[0].id = "arcadeFixOne";
        arcadeSteps[0].location = "Cryostasis Chamber";

        arcadeSteps[1].name = "Fix Arcade (Needs Wrench) ";
        arcadeSteps[1].id = "arcadeFixTwo";
        arcadeSteps[1].location = "Cryostasis Chamber";

        arcadeSteps[2].name = "Fix Arcade (Needs Wrench) ";
        arcadeSteps[2].id = "arcadeFixThree";
        arcadeSteps[2].location = "Cryostasis Chamber";

// FIX THRUSTER
        taskRepo[9].name = "Fix Thruster";
        taskRepo[9].importance = 1;
        taskRepo[9].currentStep = 0;
        taskRepo[9].steps = new Step[1];
            taskRepo[9].steps[0].id = "thrustmaster3000";
            taskRepo[9].steps[0].name = "Fix Thruster (Needs Welder)";
            taskRepo[9].steps[0].location = "Outside";

// WARM MILK
        taskRepo[10].name = "Warm Milk";
        taskRepo[10].importance = 0;
        taskRepo[10].currentStep = 0;
        taskRepo[10].steps = new Step[4];
            taskRepo[10].steps[0].id = "missionImpossible:FindDad(orGetMilk)";
            taskRepo[10].steps[0].name = "Get Milk (Needs Cup)";
            taskRepo[10].steps[0].location = "Kitchen";

            taskRepo[10].steps[1].id = "placeCup";
            taskRepo[10].steps[1].name = "Placing Cup";
            taskRepo[10].steps[1].location = "Kitchen";
            
            taskRepo[10].steps[2].id = "milkWarm";
            taskRepo[10].steps[2].name = "Heating Milk";
            taskRepo[10].steps[2].location = "Kitchen";

            taskRepo[10].steps[3].id = "drinkMilk";
            taskRepo[10].steps[3].name = "Drinking Milk";
            taskRepo[10].steps[3].location = "Kitchen";

// HUG TEDDY BEAR
        taskRepo[11].name = "Hug Teddy Bear";
        taskRepo[11].importance = 0;
        taskRepo[11].currentStep = 0;
        taskRepo[11].steps = new Step[1];
            taskRepo[11].steps[0].id = "hugBear";
            taskRepo[11].steps[0].name = "Hug Teddy Bear";
            taskRepo[11].steps[0].location = "Cryostasis Chamber";


// OLD CODE
        //if (taskRepo.Length > 0)
        //{
         //   activeTasks = new Task[taskRepo.Length];
        //}

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

        if (FindTaskOfName(taskRepo, "Warm Milk") != -1 && FindTaskOfName(taskRepo, "Hug Teddy bear") != -1 && FindTaskOfName(taskRepo, "Make Food") == -1) 
        {
            TaskAdd(FindTaskOfName(taskRepo, "Warm Milk"));
            TaskAdd(FindTaskOfName(taskRepo, "Hug Teddy bear"));
            TaskAdd(FindTaskOfName(taskRepo, "Make Food"));
        }


            for (int i = 0; i < GameManager.stage + 7; i++)
            {
                ChooseRandomTask();
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stats != null)
        {
            if (!stats.HasElectricity)
            {
                if (FindTaskOfName(activeTasks, "Fix Lights") == -1)
                {
                    TaskAdd(FindTaskOfName(taskRepo, "Fix Lights"));
                }
            }
            if (stats.Fuel < 30)
            {
                if (FindTaskOfName(activeTasks, "Refuel Ship") == -1)
                {
                    TaskAdd(FindTaskOfName(taskRepo, "Refuel Ship"));
                }
            }
        }
        if (timeController.CurrentTime < timeController.EndHour)
        {
            taskTimer += Time.deltaTime;
        }
        
        if (taskTimer >= Random.Range(minTimeToTask, maxTimeToTask))
        {
            if (countActiveTasks < taskRepo.Length-2) //this one is because of Electrical & Fuel, will need to be increeased when other non-randomly chosen tasks are implemented
            {
                ChooseRandomTask();
            }
            taskTimer = 0f;
        }

        //for each task in Active tasks, apply passive effect

        //Every n secs there is a m percent of chance a task from taskRepo is added to activeTasks

        if (DEWIT)
        {
            ChooseRandomTask();
            DEWIT = false;
        }
    }

    public int FindTaskOfName(Task[] t, string N)
    {
        if (t != null)
        {
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i].name == N)
                {
                    return i;
                }
            }
        }
        return -1;
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
                    if (activeTasks[i].steps[activeTasks[i].currentStep].id == "harvestAlgae")
                    {
                        if (objectHolder != null && objectHolder.holding != null)
                        {
                            objectHolder.SwitchObj(objectHolder.holding.name, "Algae");
                        }
                        else if (objectHolder != null && objectHolder.holding == null)
                        {
                            objectHolder.TurnOn("Algae");
                        }
                    }
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
        if (!IsNotChoosable(index))
        {
            countActiveTasks --;
        }
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
                    case "grav":
                        if(activeTasks[index].varEndMod[i] == 1)
                        {
                            movement.gravity = true;
                        }
                    break;
                }
            }
        }
        activeTasks[index] = default(Task);
    }

    public void ChooseRandomTask()
    {
        int randomTask = Random.Range(0, taskRepo.Length);
        //before adding, check if it already exists, or if it's one of those that need to be added under certain condictions
        if (activeTasks[randomTask].name == taskRepo[randomTask].name || IsNotChoosable(randomTask))
        {
            ChooseRandomTask();
        } else
        {
            Debug.Log("This is randomly chosen: " + taskRepo[randomTask].name);
            TaskAdd(randomTask);
            countActiveTasks ++; //Bear thee in mind, that this here variable does not count as parte of alle active tasques. The above outline shall not be counted here after to avoid a Stacke Overflowe
        }
    }

    public bool IsNotChoosable(int i)
    {
        if (taskRepo[i].name == "Warm Milk" ||taskRepo[i].name == "Refuel Ship")
        {
            return true;
        }
        return false;
    }


    public void TaskAdd(int index)
    {
        Task T = taskRepo[index];
        if (T.name == "Breach In Hull!")
        {
            T.steps[0] = hullSteps[Random.Range(0, hullSteps.Length)]; //Add cses for Fire and any other that has random placement
        }
        if (T.name == "Extinguish Fire")
        {
            T.steps[0] = fireSteps[Random.Range(0, fireSteps.Length)];
        }
        if (T.name == "Fix Arcade")
        {
            T.steps[0] = arcadeSteps[Random.Range(0, arcadeSteps.Length)];
        }
        if (T.name == "Fix Water Pipes")
        {
            T.steps[0] = waterSteps[Random.Range(0, waterSteps.Length)];
            T.steps[1] = waterSteps[Random.Range(0, waterSteps.Length)];
            while (T.steps[1].id == T.steps[0].id)
            {
                T.steps[1] = waterSteps[Random.Range(0, waterSteps.Length)];
            }
            T.steps[2] = waterSteps[Random.Range(0, waterSteps.Length)];
            while (T.steps[2].id == T.steps[0].id || T.steps[2].id == T.steps[1].id)
            {
                T.steps[2] = waterSteps[Random.Range(0, waterSteps.Length)];
            }
        }
        activeTasks[index] = T;
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
                    case "grav":
                        if(activeTasks[index].varStartMod[i] == 0)
                        {
                            movement.gravity = false;
                        }
                        break;
                }
            }
        }
    }
}


