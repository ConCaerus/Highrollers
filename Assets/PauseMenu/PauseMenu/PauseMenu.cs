using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    public GameObject optionsCanvas;

    public void ShowOptionsMenu()
    {
        optionsCanvas.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Debug.LogError("PauseMenuUI is not assigned in the Inspector!");
            return;
        }
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game paused");
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            OptionsCanvas.I.hide();
        }
        else
        {
            Debug.LogError("PauseMenuUI is not assigned in the Inspector!");
            return;
        }
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game resumed");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        SceneManager.LoadScene("MainMenu");
    }


}
