using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pauseKey;
    public Transform playerUI;
    public Transform pauseUI;

    private bool pausing;

    private void Update()
    {
        PauseKey();
        UnPauseKey();
    }

    public void PauseKey()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            pausing = !pausing;
            playerUI.gameObject.SetActive(false);
            pauseUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void UnPauseKey()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            pausing = !pausing;
            playerUI.gameObject.SetActive(true);
            pauseUI.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void UnPauseButton()
    {
        pausing = !pausing;
        playerUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
