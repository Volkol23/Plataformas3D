using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Text : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject finalText;

    private void Update()
    {
        pointsText.text = "Points: " + Game_Manager._GAME_MANAGER.GetPoints();
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
