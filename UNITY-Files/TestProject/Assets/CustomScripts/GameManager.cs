using UnityEngine.SceneManagement;

public class GameManager : MonoBehavior
{
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name)
    }
}