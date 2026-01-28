using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public bool gotoGameSceneAfterDelay;
    public bool gotoMainSceneAfterDelay;
    public float delay;

    public void GoToGameScene()
    {
        SceneManager.LoadScene("RespawnCutScene"); //later change to GameScene
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsCutScene");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            if (gotoGameSceneAfterDelay)
            {
                SceneManager.LoadScene("Scene with ship stats");
            }
            if (gotoMainSceneAfterDelay)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
