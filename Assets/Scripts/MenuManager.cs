using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string player;
    [SerializeField] private TMP_InputField playerName;
    //[SerializeField] private Button startButton;

    public void StartButton()
    {
        player = playerName.text;
        PlayerPrefs.SetString("PlayerName", player);
        SceneManager.LoadScene("Level1");
    }


}
