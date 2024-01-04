using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string newLevel)
    {
        if (Data.isCanGacha)
        {
            SceneManager.LoadScene(newLevel);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
