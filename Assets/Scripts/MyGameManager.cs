using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{

    bool gameEnded = false;
    public float restartDelay = 1f;
    public GameObject menuUI;
    public void completeLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Won");
    }

    public void EndGame()
    {
        gameEnded = true;
        // Invoke mainly for delays
        Debug.Log("ended");
        menuUI.SetActive(true);
        Invoke("Restart", restartDelay);

        menuUI.SetActive(false);
    }
    
    public void ClickStart()
    {
        menuUI.SetActive(false);
    }

    void Restart()
    {
        SceneManager.LoadScene("RollaBall");//(SceneManager.GetActiveScene().name);
    }


    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("RollABall");
        //SceneManager.LoadScene([1], LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
