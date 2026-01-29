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
        GameManager.stage++;
        if (GameManager.stage > GameManager.finalStage)
        {
            SceneManager.LoadScene("CreditsCutScene");
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
