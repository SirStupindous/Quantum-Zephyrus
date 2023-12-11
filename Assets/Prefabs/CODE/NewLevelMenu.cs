using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevelMenu : MonoBehaviour
{
    public XPScript xpbar;
    public TMP_Text lvlText; 
    public int xpToLevelUp = 100;

    public void Start()
    {
        xpbar.SetMaxXP(xpToLevelUp);
        // xpbar.SetXP();
        lvlText.text = xpbar.getLevel().ToString();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
