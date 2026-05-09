using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject PausePanel;
    public GameObject HUDPanel;

    void Start()
    {
        if(PausePanel != null){
            PausePanel.SetActive(false);
        }
    }

    public void StartGame(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        HUDPanel.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        HUDPanel.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}