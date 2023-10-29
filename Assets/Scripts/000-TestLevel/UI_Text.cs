using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Text : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject finalText;

    private string maxPoints;

    private void Start()
    {
        maxPoints = Game_Manager._GAME_MANAGER.GetMaxPoints().ToString();
    }
    private void Update()
    {
        //UI points update and win Game Text
        pointsText.text = "Points: " + Game_Manager._GAME_MANAGER.GetPoints() + "/" + maxPoints;
        if (Game_Manager._GAME_MANAGER.WinGame())
        {
            finalText.SetActive(true);
        }
        else
        {
            finalText.SetActive(false);
        }
    }
}
