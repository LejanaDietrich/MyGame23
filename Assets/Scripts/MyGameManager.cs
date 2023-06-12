using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MyGameManager : MonoBehaviour
{

    bool gameEnded = false;
    public float restartDelay = 1f;
    public GameObject menuUI;
    public static MyGameManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public void completeLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Won");
    }

    public void EndGame()
    {
        //gameEnded = true;
        // Invoke mainly for delays
        Debug.Log("ended");
        //menuUI.SetActive(true);
        Invoke("Restart", restartDelay);

        //menuUI.SetActive(false);
    }
    
    public void ClickStart()
    {
        //menuUI.SetActive(false);
        GameObject[] menues = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject menu in menues)
        {
            menu.SetActive(false);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene("RollaBall");//(SceneManager.GetActiveScene().name);
        //winTextObject.SetActive(false);
        UnityEngine.Object.Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("RollaBall");
        SceneManager.LoadScene("Plains", LoadSceneMode.Additive);
        SceneManager.LoadScene("Descent", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
}
