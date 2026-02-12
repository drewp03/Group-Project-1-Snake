using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void BacktoMain()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void EasyButton()
    {
        SnakeMovement.startingMoveInterval = 0.5f;
    }

    public void HardButton()
    {
        SnakeMovement.startingMoveInterval = 0.2f;
    }


    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}