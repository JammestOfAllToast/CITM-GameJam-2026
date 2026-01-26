using UnityEngine;
using DefaultNamespace;
using UnityEngine.SceneManagement;

public class CryoPod : MonoBehaviour, IInteractable
{
    public string InteractMessage => objectInteractMessage;
    public string objectInteractMessage;


    public PlayerStats stats;

    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void Interact()
    {
        if (stats != null)
        {
            if (stats.HasWon)
            {
                NextStage();
            }
        }
    }

    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.stage++;
    }
}
