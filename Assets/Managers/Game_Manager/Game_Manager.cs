using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager _GAME_MANAGER;

    private int points;
    private int totalPoints;
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
            AllPoints();
        }
    }

    private void AllPoints()
    {
        //Asign the max points of the scene
        GameObject[] points = GameObject.FindGameObjectsWithTag("Coin");
        totalPoints = points.Length;
    }

    public void ResetGame()
    {
        //Reset Main Scene
        SceneManager.LoadScene("000-TestLevel", LoadSceneMode.Single);
        points = 0;
    }

    public void UpdatePoints()
    {
        //Add points
        points++;
    }

    //Variable Getters
    public int GetPoints()
    {
        return points;
    }

    public int GetMaxPoints()
    {
        return totalPoints;
    }

    //Win Game
    public bool WinGame()
    {
        return points == totalPoints && totalPoints > 0;
    }
}
