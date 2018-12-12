using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour {

    float timer = 0;
    float timerRounded;

    public Vector2 startPos;

    public GameObject playerChar;
    public GameObject pauseUI, quitUI;
    public bool fade;

    Vector3 uiStarPos;
    Vector3 uiVisiblePos;

    Image img;
    string chosenScene;

    void Start()
    {
        Time.timeScale = 1;
        playerChar = GameObject.Find("Player");
        pauseUI = GameObject.Find("PauseUI");
        quitUI = GameObject.Find("QuitConfirm");

        img = GameObject.Find("Fader").GetComponent<Image>();
        StartCoroutine(FadeImage(true));

        uiStarPos = new Vector3(0,1600,0);
        uiVisiblePos = new Vector3(0,0,0);

        img.color = new Color(0, 0, 0, 1);
    }

    void Update()
    {
        //TO DO: make timer decimally accurate.
        timer += Time.deltaTime;
        timerRounded = Mathf.Round(timer * 100f) / 100f;
        //Debug.Log(timerRounded);

        //Pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }

    #region pause menu management
    public void SetVisiblePos(GameObject canvasItem)
    {
        canvasItem.transform.localPosition = uiVisiblePos;
    }
    public void SetInvisiblePos(GameObject canvasItem)
    {
        canvasItem.transform.localPosition = uiStarPos;
    }

    public void Pause() //pause the game and open the pause canvas.
    {
            if (Time.timeScale == 1)
            {
            Time.timeScale = 0;
            SetVisiblePos(pauseUI);      
            }        
    }

    public void Unpause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            SetInvisiblePos(pauseUI);
            SetInvisiblePos(quitUI);
        }
    }

    public void Restart(string sceneName) //Restart the game, reset any object positions to their start positions, and reset the timer.
    {
        Scene m_Scene = SceneManager.GetActiveScene();
        chosenScene = m_Scene.name;
        StartCoroutine(FadeImage(false));
        Time.timeScale = 1;
    }

    public void GoToMainMenu(string sceneName) //go to the main menu scene.
    {
        Time.timeScale = 1;
        StartCoroutine(FadeImage(false));
        chosenScene = sceneName;
    }

    public void Exit()
    {
        Application.Quit();
    }
    #endregion

    #region fade in and out coroutine
    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
            {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);

                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 2; i += Time.deltaTime * 2)
            {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);

                if (i >= 1.0f)                  
                    SceneManager.LoadScene(chosenScene);
                    yield return null;
            }
        }
    }
    #endregion

    public void NextLevel()
    {
        Scene m_Scene = SceneManager.GetActiveScene();
        if (m_Scene.name != "Level 2")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            Debug.Log("Victory!");



    }
}
