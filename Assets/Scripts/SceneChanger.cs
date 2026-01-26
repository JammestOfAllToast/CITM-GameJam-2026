using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public void GoToGameScene()
    {
        SceneManager.LoadScene("Scene with ship stats"); //later change to GameScene
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
