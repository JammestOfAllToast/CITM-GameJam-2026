using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : MonoBehaviour
{
    private TaskManager tasks;
    private TextMeshProUGUI textMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tasks = GameObject.FindWithTag("Player").GetComponent<TaskManager>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tasks.activeTasks != null && GameManager.inventoryOpen)
        {
            textMesh.text = "";
            for (int i = 0; i < tasks.activeTasks.Length; i++)
            {
                if (tasks.activeTasks[i].name != default(TaskManager.Task).name)
                {
                    if (tasks.activeTasks[i].importance == 2)
                    {
                        textMesh.color = ColorUtility.TryParseHtmlString("#081820", out Color c) ? c : Color.white;
                    } else if (tasks.activeTasks[i].importance == 1)
                    {
                        textMesh.color = ColorUtility.TryParseHtmlString("#346856", out Color c) ? c : Color.white;
                    } else
                    {
                        textMesh.color = ColorUtility.TryParseHtmlString("#88c070", out Color c) ? c : Color.white;
                    }

                    textMesh.text += tasks.activeTasks[i].name + ": [" + tasks.activeTasks[i].currentStep + " / " + tasks.activeTasks[i].steps.Length + "]\n    " + tasks.activeTasks[i].steps[tasks.activeTasks[i].currentStep].name + "\n    ->" + tasks.activeTasks[i].steps[tasks.activeTasks[i].currentStep].location + "\n";
                }
            }   
        }
    }
}
