using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Starts the Game Scene
    public void PlayGame()
    {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Game");
    }

    // Quits Game
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
