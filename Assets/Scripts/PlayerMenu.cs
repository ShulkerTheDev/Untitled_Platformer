using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    public GameObject canvasUi;
    public GameObject pauseText;

    bool gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasUi = GameObject.Find("Canvas");
        pauseText = canvasUi.transform.GetChild(0).gameObject;
    }

    public void PauseControl()
    {
      if (gamePaused == true)
      {
        ResumeGame();
        pauseText.SetActive(false);
      }
      else
      {
        PauseGame();
        pauseText.SetActive(true);
      }
    }

    public void PauseGame()
    {
      Time.timeScale = 0;

      gamePaused = true;
    }

    public void ResumeGame()
    {
      Time.timeScale = 1;
      
      gamePaused = false;
    }
}
