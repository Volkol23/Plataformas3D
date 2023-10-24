using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager _GAME_MANAGER;

    private int points;
    private void Awake()
    {
        if(_GAME_MANAGER != null && _GAME_MANAGER != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //Set up Game manager singleton
            _GAME_MANAGER = this;
            DontDestroyOnLoad(_GAME_MANAGER);

            points = 0;
        }
    }

    public void ResetGame()
    {
        //Reset Main Scene
        SceneManager.LoadScene("000-TestLevel", LoadSceneMode.Single);
        points = 0;
    }

    public void UpdatePoints()
    {
        points++;
    }

    public int GetPoints()
    {
        return points;
    }
}
