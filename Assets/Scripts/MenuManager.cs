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
    [SerializeField] private Image soundOnImage;
    [SerializeField] private Image soundOffImage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private BasePopUp menuPopUp;
    [SerializeField] private BasePopUp instructionPopUp;
    //[SerializeField] private Button startButton;

    public void StartButton()
    {
        player = playerName.text;
        PlayerPrefs.SetString("PlayerName", player);
        SceneManager.LoadScene("Level");
    }

    public void soundOnButton()
    {
        soundOnImage.gameObject.SetActive(false);
        soundOffImage.gameObject.SetActive(true);
        audioSource.Pause();
    }

    public void soundOffButton()
    {
        soundOffImage.gameObject.SetActive(false);
        soundOnImage.gameObject.SetActive(true);
        audioSource.UnPause();
    }

    public void instructionOnButton()
    {
        menuPopUp.Close();
        instructionPopUp.Open();
    }

    public void backButton()
    {
        menuPopUp.Open();
        instructionPopUp.Close();
    }
}
