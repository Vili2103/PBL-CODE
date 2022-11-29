using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static bool pausedGame=false;
    public GameObject pauseUI;


    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausedGame==false)
        {
            Pause();
        }
       else if (Input.GetKeyDown(KeyCode.Escape) && pausedGame == true)
        {
            Resume();
        }
    }
    void Pause()
    {
        
        pausedGame = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }
   public void Resume()
    {
        Time.timeScale = 1f;
        pausedGame = false;
        pauseUI.SetActive(false);
    }
}
