using UnityEngine;

public class Pause_menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool pausedGame = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pausedGame = false;
    } 

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        pausedGame = true;
    }




}
