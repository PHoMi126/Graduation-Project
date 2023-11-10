using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pauseKey;
    public Transform playerUI;
    public Transform pauseUI;
    public PlayerPOV playerPOV;

    private bool pausing;

    private void Update()
    {
        PauseKey();

        if (playerPOV.lockCursor == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void PauseKey()
    {
        if (Input.GetKeyDown(pauseKey) && pausing == false)
        {
            pausing = true;
            playerUI.gameObject.SetActive(false);
            pauseUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(pauseKey) && pausing == true)
        {
            pausing = false;
            playerUI.gameObject.SetActive(true);
            pauseUI.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void UnPauseButton()
    {
        pausing = false;
        playerUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
