using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pauseKey;
    public GameObject playerUI;
    public GameObject pauseUI;
    public PlayerPOV playerPOV;

    internal bool pausing = false;

    private void Update()
    {
        PauseKey();
    }

    public void PauseKey()
    {
        if (Input.GetKeyDown(pauseKey) && pausing == false)
        {
            pausing = true;
            playerUI.SetActive(false);
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerPOV.lockCursor = false;
        }
        else if (Input.GetKeyDown(pauseKey) && pausing == true)
        {
            pausing = false;
            playerUI.SetActive(true);
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerPOV.lockCursor = true;
        }

    }

    public void UnPauseButton()
    {
        pausing = false;
        playerUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerPOV.lockCursor = true;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
